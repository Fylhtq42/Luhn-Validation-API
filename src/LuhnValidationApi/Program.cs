using LuhnValidationApi;
using LuhnValidationApi.Exceptions;
using LuhnValidationApi.Models;
using LuhnValidationApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(); 

builder.Services.AddScoped<ILuhnValidator, LuhnValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Global error-handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (InvalidCreditCardException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new
        {
            error = ex.Message
        });
    }
    catch (Exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred."
        });
    }
});

app.MapPost("/api/validate-credit-card", (CreditCardRequest request, ILuhnValidator validator) =>
    {
        var isValid = validator.IsValid(request.CreditCardNumber);
        return Results.Ok(new { isValid });
    })
    .WithName("ValidateCreditCard");

app.Run();