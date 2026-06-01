namespace AXIOCRM.Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendAsync(string toEmail, string subject, string body);
    }
}
