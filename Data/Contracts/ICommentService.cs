using Data.Models;
using Data.Validation.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface ICommentService
    {
        Task<CommentDTO?> GetCommentById(int id, CancellationToken token = default);
        Task<IEnumerable<CommentDTO>> GetCommentsByPost(int id, CancellationToken token = default);
        Task CreateComment(CommentCreateDTO comment, int postId, int userId, CancellationToken token = default);
        Task UpdateComment(CommentCreateDTO comment, CancellationToken token = default);
        Task DeleteComment(int id, CancellationToken token = default);
    }
}
