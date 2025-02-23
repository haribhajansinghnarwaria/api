using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repoistories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
             await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentAsync()
        {
            var list = await _context.Comments.ToListAsync();

            return list;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return comment;
        }

        public async Task<Comment> UpdateCommentByIdAsync(int commentId, Comment toCommentModelfromUpdateCommentDto)
        {
            var isCommentExist = await _context.Comments.FindAsync(commentId);
            if (isCommentExist == null)
            {
                return null;
            }
            isCommentExist.Title = toCommentModelfromUpdateCommentDto.Title;
            isCommentExist.Content = toCommentModelfromUpdateCommentDto.Content;
            await _context.SaveChangesAsync();

            return toCommentModelfromUpdateCommentDto;
            
        }
    }
}
