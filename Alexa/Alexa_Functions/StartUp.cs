using Alexa_Functions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Options;
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

            services.Configure<DBSettings>(options =>
                root.GetSection("DBSettings").Bind(options));

            services.AddSingleton<IAmazonDynamoDB>(f =>
            {
                var o = f.GetService<IOptions<DBSettings>>();
                return new AmazonDynamoDBClient(o.Value.AccessKey, o.Value.SecretKey);
            });
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IGarage>(f =>
            {
                return new GarageClient(new HttpClient());
            });

            return services;
        }
    }

    public class LambdaConfiguration : ILambdaConfiguration
    {
        public static IConfigurationRoot Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        IConfigurationRoot ILambdaConfiguration.Configuration => Configuration;
    }

    public interface ILambdaConfiguration
    {
        IConfigurationRoot Configuration { get; }
    }
}
