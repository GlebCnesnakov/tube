using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Data.Models;
namespace Data.Validation;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Post>, PostValidator>();
        return services;
    }
}