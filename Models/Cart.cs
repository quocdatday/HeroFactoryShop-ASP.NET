using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Carts")]
    public class Cart : ModelBase
    {
        [Key]
        public int CAR_ID { get; set; }
        public string? USE_ID { get; set; }
        public string? Customer { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public string? IsPaid { get; set; }
        public string? Note { get; set; }
        [ForeignKey("USE_ID")]
        public User? User { get; set; }
        public ICollection<CartDetail>? CartDetails { get; set; }
    }
}
