using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Data;
using Auth;
using Data.Models;
using Data.Contracts;
using Data.Validation;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuthExtensions(builder.Configuration);
builder.Services.AddDataExtensions(builder.Configuration);
builder.Services.AddValidation();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var x = app.MapGroup("api/v1");


x.MapPost("/register", async ([FromBody] User newUser, IAuthService authService) =>
{
    await authService.Register(newUser.Login, newUser.Password);
    return TypedResults.Created();
});

x.MapPost("/login", async ([FromBody] User user, IAuthService authService) =>
{
    return TypedResults.Ok(await authService.Authenticate(user.Login, user.Password));
});


x.MapGet("/posts", async (int? page, int? limit, IPostContract postService) =>
{
    var posts = await postService.GetPosts(page, limit);
    return TypedResults.Ok(posts);
}).WithOpenApi();

x.MapPost("/posts", async ([FromBody] Post post, IPostContract services, HttpContext context, IValidator<Post> validator) =>
{
    validator.ValidateAndThrow(post);
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    services.CreatePost(post, Int32.Parse(userId));
    return TypedResults.Created();
}).WithOpenApi().RequireAuthorization();

app.Run();