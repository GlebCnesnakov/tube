using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.Extensions.Configuration;


namespace Data.Contracts
{
    public interface IPostContract
    {
        Task<Post?> GetPostById(Post post, CancellationToken token = default);
        Task<IEnumerable<Post>> GetPosts(int? page, int? limit,  CancellationToken token = default);
        Task CreatePost(Post post, int userId, CancellationToken token = default);
        Task UpdatePost(Post post, CancellationToken token = default);
        Task DeletePost(Post post, CancellationToken token = default);
    }
}
