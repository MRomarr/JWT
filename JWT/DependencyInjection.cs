using JWT.Helpers;
using System.Net.Mail;
using System.Net;

namespace JWT
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            var emailSettings = config.GetSection(EmailSettings.Section).Get<EmailSettings>() 
                ?? throw new InvalidOperationException("Email settings are not configured properly.");
            services
           .AddFluentEmail(emailSettings.SenderEmail, emailSettings.SenderName)
           .AddSmtpSender(new SmtpClient(emailSettings.SmtpServer)
           {
               Port = emailSettings.Port,
               Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
               EnableSsl = true
           });
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }

}
