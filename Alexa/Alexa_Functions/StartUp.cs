using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Home.Core.Clients.Interfaces;
using Home.Core.Clients;
using System.Net.Http;

namespace Alexa_Functions
{
    public class StartUp
    {
        public static IServiceCollection Container => ConfigureServices(LambdaConfiguration.Configuration);

        private static IServiceCollection ConfigureServices(IConfigurationRoot root)
        {
            var services = new ServiceCollection();
            services.AddTransient<IGarage>(f =>
            {
                // These are set in by the environment variables.
                // Ensure that these are set in the launch settings and within the aws lambda function.
                var endpoint = root.GetValue<string>("GarageServiceEndpoint");
                var token = root.GetValue<string>("GarageServiceToken");
                var client = new GarageClient(new HttpClient());
                client.BaseAddress = endpoint;
                client.Token = token;

                return client;
            });

            return services;
        }
    }

    public class LambdaConfiguration : ILambdaConfiguration
    {
        public static IConfigurationRoot Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        IConfigurationRoot ILambdaConfiguration.Configuration => Configuration;
    }

    public interface ILambdaConfiguration
    {
        IConfigurationRoot Configuration { get; }
    }
}
