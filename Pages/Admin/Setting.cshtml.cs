using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class SettingModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Setting? Setting { get; set; }
        public List<Setting>? Settings { get; set; }


        public SettingModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;   
            Setting = new Setting();
            Settings = [];
        }


        public async Task<IActionResult> OnGetAsync()
        {
            Settings = await _context.Settings.ToListAsync();
            return Page();
        }


        public async Task<IActionResult> OnPostAddSettingAsync([FromBody] DataSetting data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            if (data == null)
            {
                return NotFound();
            }
            Setting = new Setting
            {
                Name = data.Name,
                Value = data.Value,
                CreatedBy = user.Email!,
                UpdatedBy = user.Email
            };
            _context.Settings.Add(Setting);
            await _context.SaveChangesAsync();
            Setting = await _context.Settings.FirstOrDefaultAsync(st => st.Name == data.Name);
            return new JsonResult(new { Setting });
        }

        public async Task<IActionResult> OnPostEditSettingAsync([FromBody] DataSetting data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            if(data == null)
            {
                return NotFound();
            }
            Setting = await _context.Settings.FirstOrDefaultAsync(x => x.SET_ID == data.ID);
            if (Setting != null) {
                Setting.Name = data.Name;
                Setting.Value = data.Value;
                Setting.UpdatedDate = DateTime.Now;
                Setting.UpdatedBy = user.Email;
                _context.Settings.Update(Setting);
                await _context.SaveChangesAsync();
                return new JsonResult(new { Setting });
            }
            return NotFound();
        }
        
        public async Task<IActionResult> OnDeleteDltSettingAsync([FromBody] DataSetting data)
        {
            if (data == null)
            {
                return NotFound();
            }
            Setting = await _context.Settings.FirstOrDefaultAsync(x => x.SET_ID == data.ID);
            if (Setting != null)
            {
                _context.Settings.Remove(Setting);
                await _context.SaveChangesAsync();
                return new JsonResult(new { id = Setting.SET_ID });
            }
            return NotFound();
        }
    }
    public class DataSetting
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
