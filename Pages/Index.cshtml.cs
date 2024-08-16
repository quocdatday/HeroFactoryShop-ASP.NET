using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        private readonly ILogger<IndexModel> _logger;

        public Cart? Carts { get; set; }

        //Page
        public int quantity { get; set; } = 0;

        public IndexModel(ILogger<IndexModel> logger, AppDataContext context, UserManager<AuthUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            Carts = new Cart();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                Carts = await _context.Carts
                    .Include(x => x.CartDetails)
                    .FirstOrDefaultAsync(x => x.USE_ID == user.Id);
                if(Carts != null && Carts.CartDetails != null)
                {
                    foreach (var carts in Carts.CartDetails)
                    {
                        quantity += carts.Quantity;
                    }
                }
                else {  quantity = 0; }
                return Page();
            }
            else
            {
                quantity = 0;
            }
            return Page();
        }
        //public IActionResult OnGet()
        //{
        //    return RedirectToPage("/ProductDetail");
        //}
    }
}
