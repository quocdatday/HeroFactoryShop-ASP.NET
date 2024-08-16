using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Banners")]
    public class Banner : ModelBase
    {
        [Key]
        public int BAN_ID { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public int DisplayOrder { get; set; }
    }
}
