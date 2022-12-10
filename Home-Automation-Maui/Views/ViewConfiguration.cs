namespace Home_Automation_Maui.Views
{
    internal static class ViewConfiguration
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddTransient<MainPage>();
            services.AddTransient<SettingsPage>();
            return services;
        }
    }
}
