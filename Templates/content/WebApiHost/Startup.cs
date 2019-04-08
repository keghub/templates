using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMG.Extensions.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(ConfigureMvc).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Adds support for ASP.NET Core health checks: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
            services.AddHealthChecks(); 

            // Adds support for JWT authentication
            services.AddJwtAuthentication(Configuration)
                    
                    // Extracts the authentication data from the form data of a POST request
                    .AddFormUserExtractor() 
                    
                    // Matches incoming credentials with credentials fetched from the configuration values
                    .AddBasicUserAuthenticator() 

                    // If the delegate returns true, adds an authentication filter at global level that forbids anonymous requests
                    .RequireAuthentication(() => HostingEnvironment.IsProduction()); 

            void ConfigureMvc(MvcOptions options)
            {
                // Adds support for friendly error when an MVC action throws an uncaught exception
                options.AddExceptionHandlerFilter();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Requests matching "/health" are forwarded to the health check engine
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false
            });

            // Uses JWT authentication flow
            app.UseJwtAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
