
using FluentValidation;
using MediatR;
using Restaurants.Application.Restaurants.Queries.CheckEmailExistence;
using Restaurants.Application.Restaurants.Queries.EmailRestaurantExist;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        //private readonly IMediator _mediator;
        public CreateRestaurantCommandValidator(/*IMediator mediator*/)
        {
            //_mediator = mediator;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 4 and 100 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(dto => dto.Category)
                .NotEmpty().WithMessage("Insert a valid Category.");

            //RuleFor(dto => dto.ContactEmail)
            //.NotEmpty().WithMessage("Contact email is required.")
            //.EmailAddress().WithMessage("Please provide a valid email address.")
            //.MustAsync(BeUniqueEmail2).WithMessage("Email already exixts.");

            RuleFor(dto => dto.ContactEmail)
           .NotEmpty().WithMessage("Contact email is required.")
           .EmailAddress().WithMessage("Please provide a valid email address.");
           //.Custom((email, context) =>
           //{
           //    var x = BeUniqueEmail2(email);
           //    if (x)
           //    {
           //        context.AddFailure("ContactEmail", "Email already exists.");
           //    }
           //});

            RuleFor(dto => dto.ContactNamber)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Please provide a valid contact number.");

            RuleFor(dto => dto.PostalCode)
                .Matches(@"^\d{2}-\d{3}$").WithMessage("Please provide a valid Postcode like (XX-XXX).")
                .When(dto => !string.IsNullOrEmpty(dto.PostalCode)); // Only validate if PostalCode is provided
        }
        //private bool BeUniqueEmail(string email)
        //{
        //    var  c =   _mediator.Send(new EmailRestaurantExistQuery(email));
        //    if (c is null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private bool BeUniqueEmail2(string email)
        //{
        //    // Use the mediator to send the query
        //    var emailExists =  _mediator.Send(new CheckEmailExistenceQuery(email));
        //    if (!emailExists.Result ) { return false; }
        //    return true; // Return true if the email is unique (doesn't exist)
        //}
    }
}
