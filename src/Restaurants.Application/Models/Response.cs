

using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Restaurants.Application.Models
{
    public class Response<T> : Response
    {
        public Response(bool success, string message) : base(success, message)
        {
        }
        public Response(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }
        public Response(bool success, string message, IdentityResult result) : base(success, message, result)
        {

        }
        public Response(bool success, string message, List<ErrorItem> errors) : base(success, message, errors)
        {

        }
        public Response(bool success, string message, ValidationResult result) : base(success, message, result)
        {

        }
        public Response(bool success, string message, IdentityResult result, T data) : base(success, message, result)
        {

            Data = data;

        }
        public T? Data { get; private set; }

    }

    public class Response
    {
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public Response(bool success, string message, IdentityResult result)
        {
            Errors = new();
            Success = success;
            Message = message;
            foreach (var error in result.Errors)
            {
                Errors.Add(
                    new ErrorItem
                    {
                        Code = error.Code,
                        Description = error.Description
                    }
                    );
            }

        }
        public Response(bool success, string message, ValidationResult result)
        {
            Errors = new();
            Success = success;
            Message = message;
            foreach (var error in result.Errors)
            {
                Errors.Add(
                    new ErrorItem
                    {
                        Code = error.ErrorCode,
                        Description = error.ErrorMessage
                    }
                    );
            }

        }
        public Response(bool success, string message, List<ErrorItem> errors)
        {
            Errors = new();
            Success = success;
            Message = message;
            Errors = errors;

        }
        public int Status { get; private set; }
        public string? Message { get; private set; }
        public bool Success { get; private set; }
        public List<ErrorItem>? Errors { get; private set; }
    }
}
