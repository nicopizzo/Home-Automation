﻿namespace PizzoHomeAutomation.Views
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
