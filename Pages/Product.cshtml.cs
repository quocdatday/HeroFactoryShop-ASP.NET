using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages
{
    public class ProductModel : PageModel
    {
        private readonly AppDataContext _context;
        public IList<Category>? Categories { get; set; }
        public IList<Product>? TopProduct { get; set; }
        public ProductModel(AppDataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Categories
                .AsNoTracking()
                .Include(x => x.Products)
                .ToListAsync();
            TopProduct = await _context.Products
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Take(8).ToListAsync();
            return Page();
        }
    }
}
