using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services;
using Shared.Settings;


namespace Shared
{
    public static class ServiceRegistration
    {
        public static void AddShared(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
