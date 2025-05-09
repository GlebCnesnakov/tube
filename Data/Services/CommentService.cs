using Data.Contracts;
using Data.Models;
using Data.Validation.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.Services
{
    public class CommentService(AppContext context, IPostContract postService) : ICommentService
    {
        async public Task<CommentDTO?> GetCommentById(int id, CancellationToken token = default)
        {
            return await context.Comments
                .Where(p => p.Id == id)
                .Select(p => new CommentDTO
                {
                    Id = p.Id,
                    Text = p.Text,
                    Post = p.Post.Id,
                    Created = p.Created,
                    Updated = p.Updated,
                    CommentAuthor = p.CommentAuthor.Login
                })
                .FirstOrDefaultAsync(token);
        }
        async public Task<IEnumerable<CommentDTO>> GetCommentsByPost(int postId, CancellationToken token = default)
        {
            return await context.Comments
                .Where(p => p.PostId == postId)
                .Select(p => new CommentDTO()
                {
                    Id = p.Id,
                    Text = p.Text,
                    Post = p.Post.Id,
                    Created = p.Created,
                    Updated = p.Updated,
                    CommentAuthor = p.CommentAuthor.Login
                }).ToListAsync(token);
        }
        async public Task CreateComment(CommentCreateDTO comment, int postId, int userId, CancellationToken token = default)
        {
            Comment commentToSave = new()
            {
                Created = DateTime.UtcNow,
                CommentAuthorId = userId,
                PostId = postId,
                Text = comment.Text
            };

            await context.Comments.AddAsync(commentToSave, token);
            await context.SaveChangesAsync(token);
        }

        async public Task UpdateComment(CommentCreateDTO comment, CancellationToken token = default)
        {
            var p = await context.Comments.FirstOrDefaultAsync(x => x.Id == comment.Id);
            if (p == null) throw new KeyNotFoundException($"Comment with id {comment.Id} not found");
            p.Text = comment.Text;
            p.Updated = DateTime.UtcNow;
            await context.SaveChangesAsync(token);
        }

        async public Task DeleteComment(int id, CancellationToken token = default)
        {
            var p = new Comment()
            {
                Id = id
            };
            context.Comments.Remove(p);
            try
            {
                await context.SaveChangesAsync(token);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new KeyNotFoundException($"Comment with id {id} not found");
            }
        }
    }
}
