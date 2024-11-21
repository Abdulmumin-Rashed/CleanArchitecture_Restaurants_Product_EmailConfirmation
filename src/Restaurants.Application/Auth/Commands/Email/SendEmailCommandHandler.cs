using MediatR;
using Restaurants.Application.Models;
using Restaurants.Domain.Interfaces;


namespace Restaurants.Application.Auth.Commands.Email
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Response>
    {
        private readonly IEmailService _emailService;

        public SendEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task<Response> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Call the synchronous SendEmail method
                _emailService.SendEmail(request.EmailMessage, request.EmailAddress);
                return Task.FromResult(new Response(true, "Email sent successfully."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Response(false, "Failed to send email.", new List<ErrorItem>
            {
                new ErrorItem { Code = "EmailError", Description = ex.Message }
            }));
            }
        }
    }

}
