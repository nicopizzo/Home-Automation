using Home.Core.Clients;
using Home.Core.Clients.Interfaces;

namespace PizzoHomeAutomation.Services
{
    internal static class ServiceConfiguration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IToastService, ToastService>();
            services.AddSingleton<ISettingService>(new SettingService(Preferences.Default));
            services.AddSingleton<IGarageService, GarageService>();
            services.AddSingleton<IGarage, GarageClient>(f =>
            {
                var settings = f.GetRequiredService<ISettingService>();
                return new GarageClient(settings.BaseEndpoint, settings.Token);
            });

            return services;
        }
    }
}
