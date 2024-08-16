using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Products")]
    public class Product : ModelBase
    {
        [Key]
        public int PRO_ID { get; set; }
        public int CAT_ID { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public decimal Price { get; set; }
        public double Rate { get; set; }
        [ForeignKey("CAT_ID")]
        public Category? Category { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<CartDetail>? CartDetails { get; set; }
    }
}
