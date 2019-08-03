using System.Device.Gpio;
using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Garage.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Home.Core.Gpio;
using Home.Core.Security;

namespace Garage.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(f => new GarageConfig() { TogglePin = Configuration.GetValue<int>("TogglePin"), ClosedPin = Configuration.GetValue<int>("ClosedPin") });
            services.AddSingleton<IGpioController, BashGpioController>();
            services.AddSingleton<IGarageRepo, GarageRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseMiddleware<TokenVerification>(Configuration.GetValue<string>("TokenKey"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}