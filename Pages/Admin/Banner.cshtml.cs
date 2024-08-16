using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using ShopOnline.Pages.admin;

namespace ShopOnline.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class BannerModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Banner? Banner { get; set; }
        public IList<Banner>? Banners {  get; set; }
        public BannerModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            Banners = new List<Banner>();
        }
        public async Task OnGetAsync()
        {
            Banners = await _context.Banners.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAddBannerAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var data = new DataBanner
                {
                    Image = Request.Form.Files["Image"],
                    Title = Request.Form["Title"],
                    Url = Request.Form["Url"],
                    DisplayOrder = int.Parse(Request.Form["DisplayOrder"]!),
                };
                if (data != null)
                {
                    Banner = new Banner
                    {
                        Title = data.Title,
                        Url = data.Url,
                        DisplayOrder = data.DisplayOrder,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Email!,
                    };
                    if (data.Image != null)
                    {
                        var filename = Path.GetFileName(data.Image.FileName);
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/banners", filename);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            data.Image.CopyTo(stream);
                        }
                        Banner.Image = filename;
                    }
                    await _context.Banners.AddAsync(Banner);
                    await _context.SaveChangesAsync();
                    Banner = await _context.Banners.FirstOrDefaultAsync(x => x.Title == data.Title);
                    return new JsonResult(new { Banner });
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostEditBannerAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                var data = new DataBanner
                {
                    ID = int.Parse(Request.Form["ID"]!),
                    Image = Request.Form.Files["Image"],
                    Title = Request.Form["Title"],
                    Url = Request.Form["Url"],
                    DisplayOrder = int.Parse(Request.Form["DisplayOrder"]!),
                };
                if (data != null) 
                {
                    Banner = await _context.Banners.FirstOrDefaultAsync(x => x.BAN_ID == data.ID);
                    if (Banner != null)
                    {
                        Banner.Title = data.Title;
                        Banner.Url = data.Url;
                        Banner.DisplayOrder = data.DisplayOrder;
                        Banner.UpdatedDate = DateTime.Now;
                        Banner.UpdatedBy = user.Email;
                        if(data.Image != null)
                        {
                            //DELETE IMAGE
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/banners", Banner.Image!);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            //

                            //UPDATE IMAGE
                            var filename = Path.GetFileName(data.Image.FileName);
                            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/banners", filename);
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                data.Image.CopyTo(stream);
                            }
                            Banner.Image = filename;
                            //
                        }
                        _context.Banners.Update(Banner);
                        await _context.SaveChangesAsync();
                        return new JsonResult(new {Banner});
                    }
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> OnDeleteDltBannerAsync([FromBody] DataBanner data)
        {
            if (data != null)
            {
                Banner = await _context.Banners.FirstOrDefaultAsync(x => x.BAN_ID == data.ID)!;
                if (Banner != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/banners", Banner.Image!);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _context.Banners.Remove(Banner);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { id = Banner.BAN_ID });
                }
            }
            return NotFound();
        }
    }

    public class DataBanner
    {
        public int ID { get; set; }
        public IFormFile? Image { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public int DisplayOrder { get; set; }
    }
}
