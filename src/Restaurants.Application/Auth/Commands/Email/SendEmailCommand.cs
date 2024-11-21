using MediatR;
using Restaurants.Application.Models;
using Restaurants.Domain.HelperModels;

namespace Restaurants.Application.Auth.Commands.Email
{
    public class SendEmailCommand : IRequest<Response>
    {
        public Message EmailMessage { get; set; }
        public string EmailAddress { get; set; }

        public SendEmailCommand(Message emailMessage, string emailAddress)
        {
            EmailMessage = emailMessage;
            EmailAddress = emailAddress;
        }
    }
}
