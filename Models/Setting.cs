using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Settings")]
    public class Setting : ModelBase
    {
        [Key]
        public int SET_ID { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
