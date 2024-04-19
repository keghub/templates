//#if (AddNybusBridge)
using Amazon.SimpleNotificationService;
using EMG.Common;
//#endif
using EMG.Extensions.AspNetCore;
//#if (AddWcfDiscovery)
using EMG.Extensions.DependencyInjection.Discovery;
//#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//#if (AddNybus)
using Nybus;
//#endif

namespace WebApiHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(ConfigureMvc);

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
//#if (AddNybus)
            
            // Configures Nybus to use RabbitMQ engine and fetch settings from the configuration values
            services.AddNybus(nybus =>
            {
                nybus.UseConfiguration(Configuration);
                
                nybus.UseRabbitMqBusEngine(rabbitMq =>
                {
                    rabbitMq.UseConfiguration();
                    rabbitMq.Configure(cfg => cfg.UnackedMessageCountLimit = 10);
                });
            });

            // Configures the NybusHostedService so that Nybus is started when the web application is started
            services.AddHostedService<NybusHostedService>();
            //#endif

            //#if (AddDiscoveryAdapter)
            services.ConfigureServiceDiscovery(Configuration.GetSection("Discovery"));
            services.AddServiceDiscoveryAdapter();
            //#endif
            //#if (AddNybusBridge || ConfigureAWS)

            // Configures AWS using the configuration values
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
//#endif
//#if (AddNybusBridge)
            
            // Registers the SNS client
            services.AddAWSService<IAmazonSimpleNotificationService>();

            services.AddSingleton<INybusBridge, SnsNybusBridge>();
//#endif
            
            void ConfigureMvc(MvcOptions options)
            {
                // Adds support for friendly error when an MVC action throws an uncaught exception
                options.AddExceptionHandlerFilter();
                options.EnableEndpointRouting = false;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
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
