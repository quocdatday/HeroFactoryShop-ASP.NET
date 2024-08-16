using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Pages
{
    [Authorize(Roles = "admin")]
    public class IndexModelAdmin : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModelAdmin(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
