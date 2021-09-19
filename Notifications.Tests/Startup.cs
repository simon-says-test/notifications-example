using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Common.Interfaces;
using Notifications.DataAccess;
using Notifications.DataAccess.Access;
using Notifications.Services;

namespace Notifications.Tests
{
    public class Startup
    {
        public static IConfiguration Configuration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(Assembly.Load("Notifications"))
                .AddControllersAsServices();

            services.AddDbContext<NotificationsDbContext>
                (options => options.UseSqlServer(Configuration().GetConnectionString("NotificationsContext")));

            services.AddTransient<INotificationsAccess, NotificationsAccess>();
            services.AddTransient<INotificationsService, NotificationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
