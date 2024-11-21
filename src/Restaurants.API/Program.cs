using Microsoft.AspNetCore.Builder;
using Restaurants.API.Extentions;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.AddPresentaion();


var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddle>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }