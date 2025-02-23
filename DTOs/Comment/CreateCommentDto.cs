using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title should have 5 or more characters")]
        [MaxLength(20, ErrorMessage = "Title should not have more than 20 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Title should have 5 or more characters")]
        [MaxLength(20, ErrorMessage = "Title should not have more than 20 characters")]
        public string Content { get; set; } = string.Empty;

    }
}
