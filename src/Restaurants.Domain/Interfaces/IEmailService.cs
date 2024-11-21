using Restaurants.Domain.HelperModels;


namespace Restaurants.Domain.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message, string email);
       // Task SendEmailAsync(Message message);

    }
}
