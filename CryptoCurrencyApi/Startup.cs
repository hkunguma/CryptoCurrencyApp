using CryptoCurrencyApi.DAL.EF;
using CryptoCurrencyApi.DAL.EF.Repository;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Configuration;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCurrencyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(n =>
            {
                n.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                });
            });

            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoCurrencyApi", Version = "v1" });
                //c.DescribeAllEnumsAsStrings();
            });
            
            // explicit opt-in - needs to be placed after AddSwaggerGen(),
            // to ensure that Newtonsoft settings/attributes are automatically honored by the Swagger generator:
            services.AddSwaggerGenNewtonsoftSupport();
                
            services.AddMemoryCache();
            services.AddDbContext<CoinbaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<CoinbaseConfig>(Configuration.GetSection("Coinbase"));

            var coinbaseConfig = new CoinbaseConfig();
            Configuration.GetSection("Coinbase").Bind(coinbaseConfig);

            services.AddSingleton<CoinbaseConfig>(coinbaseConfig);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IExchangeRate, ExchangeRate>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoCurrencyApi v1");
                    c.RoutePrefix = string.Empty; });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
