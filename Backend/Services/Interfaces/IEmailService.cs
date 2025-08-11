namespace ProductManager.API.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendAsync(string toEmail, string subject, string body);
    }
}
