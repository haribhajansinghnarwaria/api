using api.DTOs.Comment;
using api.Models;

namespace api.DTOMappers
{
    public static class UpdateDtoToComment
    {
        public static Comment ToCommentModelfromUpdateCommentDto(this UpdateCommentDto dto)
        {
            return new Comment
            {
                Title = dto.Title,
                Content = dto.Content,
            };

        }
    }
}
