using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int CommentId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty ;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }//navigational properties as StockId will allow us to access any Stock and other properties of stock.
        
        public Stock? Stock { get; set; }//This is the actuall whole table which will contain whole details of Stock with particualr stockid
    }
}