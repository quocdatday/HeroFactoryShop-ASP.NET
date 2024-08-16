using Microsoft.AspNetCore.Identity;

namespace ShopOnline.Models
{
    public class AuthUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime CreatedDate { get; set; }
    }
}
