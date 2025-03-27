using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        public AppUser AppUser { get; set; }//navigational property
        public Stock Stock { get; set; }//nav property

        //navigational property is for developer's use.
       //This is like joining two tables.
    }
}
