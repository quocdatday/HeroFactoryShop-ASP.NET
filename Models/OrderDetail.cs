using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("OrderDetails")]
    public class OrderDetail : ModelBase
    {
        [Key]
        public int ORDD_ID { get; set; }
        public int PRO_ID { get; set; }
        public int ORD_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("PRO_ID")]
        public Product? Product { get; set; }
        [ForeignKey("ORD_ID")]
        public Order? Order { get; set; }
    }
}
