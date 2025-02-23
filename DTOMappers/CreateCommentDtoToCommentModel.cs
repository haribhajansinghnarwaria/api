using api.DTOs.Comment;
using api.Models;

namespace api.DTOMappers
{
    public static class CreateCommentDtoToCommentModel
    {
        public static Comment ToCommentModelfromCreatedCommentDto( this CreateCommentDto dto, int stockId)
        {
            return new Comment { 
            Title = dto.Title,
            Content = dto.Content,  
            StockId = stockId
            };

        }
    }
}
