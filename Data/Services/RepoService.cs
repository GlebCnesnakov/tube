using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public class PostService(AppContext context) : IPostContract
    {
        public async Task<Post?> GetPostById(Post post, CancellationToken token = default)
        {
            return await context.Posts.FindAsync(new object[] { post.Id }, token);
        }

        public async Task<IEnumerable<Post>> GetPosts(int? page, int? limit, CancellationToken token = default)
        {
            var query = context.Posts.AsQueryable();
            if (!(page.HasValue && limit.HasValue)) return await query.ToListAsync(token);
            int climit = limit ?? 5;
            int cpage = ((page ?? 1) - 1) * climit;
            return await query.Skip(cpage * climit).Take(climit).ToListAsync(token);
        }

        public async Task CreatePost(Post post, int userId, CancellationToken token)
        {
            Post newPost = new Post()
            {
                Title = post.Title,
                Text = post.Text,
                AuthorId = userId,
                Created = DateTime.Now,
                Updated = null
            };
            await context.Posts.AddAsync(newPost, token);
            await context.SaveChangesAsync(token);
        }
        
        public async Task UpdatePost(Post post, CancellationToken token = default)
        {
            context.Posts.Update(post);
            await context.SaveChangesAsync(token);
        }

        public async Task DeletePost(Post post, CancellationToken token = default)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync(token);
        }
    }
}
