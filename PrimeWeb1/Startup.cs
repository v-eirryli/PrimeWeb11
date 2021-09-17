using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeWeb1
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrimeWeb1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeWeb1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async (context) =>
            {
                if (context.Request.Path.Value.Contains("checkprime"))
                {
                    int numberToCheck;
                    try
                    {
                        numberToCheck = int.Parse(context.Request.QueryString.Value.Replace("?", ""));
                        var primeService = new PrimeService();
                        if (primeService.IsPrime(numberToCheck))
                        {
                            await context.Response.WriteAsync(numberToCheck + " is prime!");
                        }
                        else
                        {
                            await context.Response.WriteAsync(numberToCheck + " is NOT prime!");
                        }
                    }
                    catch
                    {
                        await context.Response.WriteAsync("Pass in a number to check in the form /checkprime?5");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Hello World! To check if a number is prime, provide URL of the form /checkprime?5");
                }
            });

        }
    }
}
