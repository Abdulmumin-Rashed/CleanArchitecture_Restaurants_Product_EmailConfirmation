using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;


namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {

            //var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            //// Register MediatR with services from the application assembly
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

            //// Register AutoMapper with profiles from the application assembly
            //services.AddAutoMapper(applicationAssembly);

            //// Register FluentValidation with automatic validation support in ASP.NET Core
            //services.AddValidatorsFromAssembly(applicationAssembly)
            //    .AddFluentValidationAutoValidation()   // Ensures automatic validation in ASP.NET Core
            //    .AddFluentValidationClientsideAdapters();


            var applicationAssemply = typeof(ServiceCollectionExtensions).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssemply));
            services.AddAutoMapper(applicationAssemply);
            services.AddValidatorsFromAssembly(applicationAssemply)
                .AddFluentValidationAutoValidation();

            services.AddScoped<IUserContext, UserContext>();
            services.AddHttpContextAccessor();
        }
    }
}
