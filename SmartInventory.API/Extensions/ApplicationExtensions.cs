using FluentValidation;
using FluentValidation.AspNetCore;
using SmartInventory.Application.Validators;

namespace SmartInventory.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}