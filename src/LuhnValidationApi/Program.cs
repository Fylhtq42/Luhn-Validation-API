using LuhnValidationApi.Exceptions;
using LuhnValidationApi.Models;
using LuhnValidationApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<ILuhnValidator, LuhnValidator>();
builder.Services.AddScoped<ICreditCardService, CreditCardService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (InvalidCreditCardException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    }
});

app.MapPost("/api/validate-credit-card", (CreditCardRequest request, ICreditCardService cardService) =>
    {
        var isValid = cardService.IsValidCreditCard(request.CreditCardNumber);
        return Results.Ok(new { isValid });
    })
    .WithName("ValidateCreditCard");

app.Run();