using Blazored.LocalStorage;
using Blazored.Toast;

namespace PizzoHomeAutomation_Blazor.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddBlazorClientServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        return services;
    }
}
