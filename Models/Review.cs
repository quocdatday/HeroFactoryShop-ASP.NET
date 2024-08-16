using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Reviews")]
    public class Review : ModelBase
    {
        [Key]
        public int REV_ID { get; set; }
        public string? USE_ID { get; set; }
        public int PRO_ID { get; set; }
        public double Rate { get; set; }
        public string? Comment { get; set; }
        [ForeignKey("PRO_ID")]
        public Product? Product { get; set; }
        [ForeignKey("USE_ID")]
        public User? User { get; set; }
    }
}
