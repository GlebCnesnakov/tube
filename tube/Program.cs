using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Data;
using Auth;
using Data.Models;
using Data.Contracts;
using Data.Validation;
using Data.Validation.DTO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Hosting;
using Data.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddProblemDetails();

builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();


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
var posts = x.MapGroup("/posts");


x.MapPost("/register", async (User newUser, IAuthService authService) =>
{
    await authService.Register(newUser.Login, newUser.Password);
    return TypedResults.Created();
});

x.MapPost("/login", async (User user, IAuthService authService) =>
{
    return TypedResults.Ok(await authService.Authenticate(user.Login, user.Password));
});


posts.MapGet("", async (int? page, int? limit, IPostContract postService) =>
{
    var posts = await postService.GetPosts(page, limit);
    return TypedResults.Ok(posts);
}).WithOpenApi();


posts.MapGet("/{id:int}", async (int id, IPostContract postService) =>
{
    var post = await postService.GetPostById(id);
    if (post == null)
    {
        return Results.NotFound();
    }
    return TypedResults.Ok(post);
}).WithOpenApi(); 


posts.MapPost("", async (PostCreateDTO post, IPostContract services, HttpContext context, IValidator<PostCreateDTO> validator) =>
{
    validator.ValidateAndThrow(post);
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    await services.CreatePost(post, Int32.Parse(userId));
    return TypedResults.Created();
}).WithOpenApi().RequireAuthorization();


posts.MapPut("/{id:int}", async (int id, PostCreateDTO post, IPostContract services, IValidator<PostCreateDTO> validator) =>
{
    post.Id = id;
    validator.ValidateAndThrow(post);
    await services.UpdatePost(post);
    return TypedResults.Ok();
}).WithOpenApi().RequireAuthorization();

posts.MapDelete("/{id:int}", async (int id, IPostContract postService) =>
{
    await postService.DeletePost(id);
    return TypedResults.Ok();
}).WithOpenApi().RequireAuthorization();

posts.MapGet("/{postId:int}/comments", async (int postId, ICommentService commentService) =>
{
    var comments = await commentService.GetCommentsByPost(postId);
    return TypedResults.Ok(comments);
}).WithOpenApi();

posts.MapPost("/{postId:int}/comments", async (int postId, CommentCreateDTO comment, ICommentService commentService, HttpContext context, IValidator<CommentCreateDTO> validator) =>
{
    validator.ValidateAndThrow(comment);
    var userId = int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    await commentService.CreateComment(comment, postId, userId);
    return TypedResults.Created();
}).WithOpenApi().RequireAuthorization();


posts.MapPut("/comments/{commentId:int}", async (int commentId, CommentCreateDTO comment, ICommentService commentService, IValidator<CommentCreateDTO> validator) =>
{
    validator.ValidateAndThrow(comment);
    comment.Id = commentId;
    await commentService.UpdateComment(comment);
    return TypedResults.Ok();
}).WithOpenApi().RequireAuthorization();


posts.MapDelete("/comments/{commentId:int}", async (int commentId, ICommentService commentService) =>
{
    await commentService.DeleteComment(commentId);
    return TypedResults.Ok();
}).WithOpenApi().RequireAuthorization();

app.Run();