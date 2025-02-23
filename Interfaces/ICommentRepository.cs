using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentByIdAsync(int commentId, Comment toCommentModelfromUpdateCommentDto);
    }
}
