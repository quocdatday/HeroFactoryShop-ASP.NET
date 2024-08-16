using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Orders")]
    public class Order : ModelBase
    {
        [Key]
        public int ORD_ID { get; set; }
        public string? USE_ID { get; set; }
        public string? Customer { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
        public string? Note { get; set; }
        [ForeignKey("USE_ID")]
        public User? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
