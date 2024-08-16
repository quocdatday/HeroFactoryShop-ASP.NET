using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages.admin
{
    [Authorize(Roles = "admin")]
    public class ProductModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Product? Product { get; set; }
        public IList<Product>? Products { get; set; }
        public IList<Category>? Categories { get; set; }
        public ProductModel(AppDataContext context, UserManager<AuthUser> userManager) { 
            _context = context;
            _userManager = userManager;
            Products = new List<Product>();
            Categories = new List<Category>();
        }
        public async Task OnGetAsync()
        {
            Products = await _context.Products.Include(c => c.Category).AsTracking().ToListAsync();
            Categories = await _context.Categories.AsTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAddProduct()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var data = new DataPro
                {
                    Image = Request.Form.Files["Image"],
                    CATID = int.Parse(Request.Form["CATID"]!),
                    Title = Request.Form["Title"],
                    Subtitle = Request.Form["Subtitle"],
                    Price = decimal.Parse(Request.Form["Price"]!),
                };
                if (data != null)
                {
                    Product = new Product
                    {
                        CAT_ID = data.CATID,
                        Title = data.Title!,
                        Subtitle = data.Subtitle!,
                        Price = data.Price,
                        Rate = 0,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Email!,
                    };
                    if (data.Image != null)
                    {
                        var filename = Path.GetFileName(data.Image.FileName);
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", filename);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            data.Image.CopyTo(stream);
                        }
                        Product.Image = filename;
                    }
                    await _context.Products.AddAsync(Product);
                    await _context.SaveChangesAsync();
                    Product = await _context.Products
                        .Where(x => x.Title!.Equals(data.Title))
                        .FirstOrDefaultAsync()!;
                    return new JsonResult(new
                    {
                        namecat = Request.Form["Namecat"],
                        Product
                    });
                }
            }
            return Page();
        }


        public async Task<IActionResult> OnPostEditProductAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var data = new DataPro
                {
                    ID = int.Parse(Request.Form["ID"]!),
                    Image = Request.Form.Files["Image"],
                    CATID = int.Parse(Request.Form["CATID"]!),
                    Title = Request.Form["Title"],
                    Subtitle = Request.Form["Subtitle"],
                    Price = decimal.Parse(Request.Form["Price"]!),
                };
                if(data != null)
                {
                    Product = await _context.Products.FirstOrDefaultAsync(x => x.PRO_ID == data.ID);
                    if (Product != null)
                    {
                        Product.CAT_ID = data.CATID;
                        Product.Title = data.Title;
                        Product.Subtitle = data.Subtitle;
                        Product.Price = data.Price;
                        Product.UpdatedDate = DateTime.Now;
                        Product.UpdatedBy = user.Email;
                        if(data.Image != null)
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", Product.Image!);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            var filename = Path.GetFileName(data.Image.FileName);
                            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", filename);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                data.Image.CopyTo(stream);
                            }
                            Product.Image = filename;
                        }
                        _context.Products.Update(Product);
                        await _context.SaveChangesAsync();
                        return new JsonResult(new
                        {
                            namecat = Request.Form["Namecat"],
                            Product
                        });
                    }
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnDeleteDltProductAsync([FromBody] DataPro data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            Product = await _context.Products.FirstOrDefaultAsync(x => x.PRO_ID == data.ID)!;
            if (Product != null) 
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", Product.Image!);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();
                return new JsonResult(new { id = Product.PRO_ID });
            }
            return NotFound();
        }
    }
    public class DataPro
    {
        public int ID { get; set; }
        public int CATID { get; set; }
        public IFormFile? Image { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public decimal Price { get; set; }
    }
}
