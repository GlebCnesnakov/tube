using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Data.Models;
using Data.Validation.DTO;
namespace Data.Validation;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Post>, PostValidator>();
        services.AddScoped<IValidator<Comment>, CommentValidator>();
        services.AddScoped<IValidator<CommentCreateDTO>, CommentCreateDTOValidator>();
        services.AddScoped<IValidator<PostCreateDTO>, PostCreateDTOValidator>();
        
        //services.AddValidatorsFromAssemblyContaining<PostValidator>();
        return services;
    }
}
