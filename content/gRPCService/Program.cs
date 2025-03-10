using gRPCService.Services;

namespace gRPCService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddAppSecrets();

            // Configure logging
            builder.Services.AddLogging(logging =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    logging.AddDebug();
                }

                if (builder.Environment.IsProduction())
                {
                    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                    logging.AddLoggly(builder.Configuration.GetSection("Loggly"));
                    logging.AddJsonConsole();
                }
            });

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddGrpcReflection();

            var app = builder.Build();

            // Use GrpcWeb
            app.UseGrpcWeb();
            app.MapGrpcReflectionService();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<ExampleService>().EnableGrpcWeb();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}