using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Models
{
    [Table("Categories")]
    public class Category : ModelBase
    {
        [Key]
        public int CAT_ID { get; set; }
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
