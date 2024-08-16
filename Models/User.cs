using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    //[Table("Users")]
    public class User : ModelBase
    {
        [Key]
        [Column("Id")]
        public string? USE_ID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Cart>? Carts { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
