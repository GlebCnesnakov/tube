using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Validation.DTO;
using Microsoft.Extensions.Configuration;


namespace Data.Contracts
{
    public interface IPostContract
    {
        Task<PostDTO?> GetPostById(int id, CancellationToken token = default);
        Task<IEnumerable<PostDTO>> GetPosts(int? page, int? limit,  CancellationToken token = default);
        Task CreatePost(PostCreateDTO post, int userId, CancellationToken token = default);
        Task UpdatePost(PostCreateDTO post, CancellationToken token = default);
        Task DeletePost(int id, CancellationToken token = default);
    }
}
