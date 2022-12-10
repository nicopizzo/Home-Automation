﻿namespace Home_Automation_Maui.ViewModels
{
    internal static class ViewModelConfiguration
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<SettingsViewModel>();
            return services;
        }
    }
}
