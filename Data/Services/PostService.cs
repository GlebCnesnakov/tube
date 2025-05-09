using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Validation.DTO;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public class PostService(AppContext context) : IPostContract
    {
        public async Task<PostDTO?> GetPostById(int id, CancellationToken token = default)
        {
            return await context.Posts
                .Where(p => p.Id == id)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    Author = p.Author.Login,
                    Created = p.Created,
                    Updated = p.Updated,
                    Comments =  p.Comments
                })
                .FirstOrDefaultAsync(token);
            
        }

        public async Task<IEnumerable<PostDTO>> GetPosts(int? page, int? limit, CancellationToken token = default)
        {
            var query = context.Posts.Include(x => x.Comments).AsQueryable();
            if (!(page.HasValue && limit.HasValue)) return await query.Select(p => new PostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Text = p.Text,
                Author = p.Author.Login,
                Created = p.Created,
                Updated = p.Updated,
                Comments =  p.Comments
            }).ToListAsync(token);
            int climit = limit ?? 5;
            int cpage = ((page ?? 1) - 1) * climit;
            return await query.Skip(cpage * climit).Take(climit).Select(p => new PostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Text = p.Text,
                Author = p.Author.Login,
                Created = p.Created,
                Updated = p.Updated,
                Comments =  p.Comments
            }).ToListAsync(token);
        }

        public async Task CreatePost(PostCreateDTO post, int userId, CancellationToken token = default)
        {
            Post newPost = new Post()
            {
                Title = post.Title,
                Text = post.Text,
                AuthorId = userId,
                Created = DateTime.UtcNow,
                Updated = null
            };
            await context.Posts.AddAsync(newPost, token);
            await context.SaveChangesAsync(token);
        }
        
        public async Task UpdatePost(PostCreateDTO post, CancellationToken token = default)
        {
            var p = await context.Posts.FirstOrDefaultAsync(x => x.Id == post.Id);
            if (p == null) throw new KeyNotFoundException($"Post with id {post.Id} not found");
            p.Text = post.Text;
            p.Updated = DateTime.UtcNow;
            p.Title = post.Title;
            await context.SaveChangesAsync(token);
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            var p = new Post()
            {
                Id = id
            };
            context.Posts.Remove(p);
            try
            {
                await context.SaveChangesAsync(token);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new KeyNotFoundException($"Post with id {id} not found");
            }
        }
    }
}
