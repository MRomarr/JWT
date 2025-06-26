namespace JWT.Services
{
    using FluentEmail.Core;

    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _email;

        public EmailService(IFluentEmail email)
        {
            _email = email;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _email
                .To(to)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();
        }
    }

}
