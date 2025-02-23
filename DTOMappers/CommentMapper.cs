using api.DTOs.Comment;
using api.Models;

namespace api.DTOMappers
{
    public static class CommentMapper
    {
        public static CommentDto CommentModelToDto(this Comment _comment)
        {
            return new CommentDto {  
                CommentId = _comment.CommentId,
                Content = _comment.Content,
                CreatedOn = _comment.CreatedOn,
                StockId = _comment.StockId,
                Title = _comment.Title,         
            };

        }
    }
}
