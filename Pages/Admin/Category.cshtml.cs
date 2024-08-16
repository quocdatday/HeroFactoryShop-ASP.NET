using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class CategoryModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public List<Category>? Categories { get; set; } = [];
        public Category? Category { get; set; } = new Category();
        public CategoryModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public void OnGet()
        {
            Categories = _context.Categories.AsTracking().ToList();
        }


        public async Task<IActionResult> OnPostAddCategoryAsync([FromBody] DataCate data)
        {
            var user = _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Challenge();
            }
            if (data == null)
            {
                return BadRequest();
            }
            Category = new Category
            {
                Name = data.Name,
                CreatedBy = user.Result!.Email!,
            };
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            Category = _context.Categories.FirstOrDefault(n => n.Name == data.Name)!;
            return new JsonResult(new { Category });
        }

        public async Task<IActionResult> OnPostEditCategoryAsync([FromBody] DataCate data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if(data != null)
                {
                    Category = await _context.Categories.FirstOrDefaultAsync(x => x.CAT_ID == data.ID);
                    if (Category != null)
                    {
                        Category.Name = data.Name;
                        Category.UpdatedDate = DateTime.Now;
                        Category.UpdatedBy = user.Email;
                        _context.Categories.Update(Category);
                        await _context.SaveChangesAsync();
                        return new JsonResult(new { Category });
                    }
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> OnDeleteDltCategoryAsync([FromBody] DataCate data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            Category = _context.Categories.FirstOrDefault(x => x.CAT_ID == data.ID)!;
            _context.Categories.Remove(Category!);
            await _context.SaveChangesAsync();
            return new JsonResult(new { id = Category.CAT_ID });
        }
    }

    public class DataCate
    {
        public int ID { get; set; }
        public string? Name { get; set; }
    }
}