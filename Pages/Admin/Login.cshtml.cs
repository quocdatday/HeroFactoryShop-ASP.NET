using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages.admin
{
    [Authorize(Roles = "admin")]
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
