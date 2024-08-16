using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class ReviewModel : PageModel
    {
        private readonly AppDataContext _context;
        public Review? Review { get; set; }
        public IList<Review>? Reviews { get; set; } = [];
        public ReviewModel(AppDataContext context) 
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Reviews = await _context.Reviews.Include(x => x.Product).AsNoTracking().ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnDeleteDltCategoryAsync([FromBody] DataRev data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            Review = await _context.Reviews.FirstOrDefaultAsync(x => x.REV_ID == data.ID);
            if(Review != null)
            {
                _context.Reviews.Remove(Review);
                await _context.SaveChangesAsync();
                return new JsonResult(new { id = Review.REV_ID });
            }
            return NotFound();
        }
    }
    public class DataRev
    {
        public int ID { get; set; }
    }
}
