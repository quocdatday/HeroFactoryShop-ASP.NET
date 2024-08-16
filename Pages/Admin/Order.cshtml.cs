using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class OrderModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Order? Order { get; set; }
        public IList<Order>? Orders;
        public IList<OrderDetail>? OrderDetails { get; set; } = [];

        [BindProperty]
        public int EditId { get; set; }

        public OrderModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            Orders = new List<Order>();
            Order = new Order();
        }

        public void OnGet()
        {
            Orders = _context.Orders.AsNoTracking().ToList();
        }

        public async Task<IActionResult> OnDeleteDltOrderAsync([FromBody] DataOrder data)
        {
            if (data != null)
            {
                Order = await _context.Orders.FirstOrDefaultAsync(x => x.ORD_ID == data.ID);
                if (Order != null)
                {
                    OrderDetails = await _context.OrderDetails.Where(x => x.ORD_ID == Order.ORD_ID).ToListAsync();
                    _context.OrderDetails.RemoveRange(OrderDetails);
                    await _context.SaveChangesAsync();
                    _context.Orders.Remove(Order);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { id = Order.ORD_ID });
                    //return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAccept()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Order = await _context.Orders.FirstOrDefaultAsync(x => x.ORD_ID == EditId);
                if (Order != null)
                {
                    Order.IsPaid = true;
                    Order.UpdatedDate = DateTime.Now;
                    Order.UpdatedBy = user.Email;
                    _context.Orders.Update(Order);
                    await _context.SaveChangesAsync();
                    return RedirectToPage();
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> OnPostEditCancel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Order = await _context.Orders.FirstOrDefaultAsync(x => x.ORD_ID == EditId);
                if (Order != null)
                {
                    Order.IsPaid = false;
                    Order.UpdatedDate = DateTime.Now;
                    Order.UpdatedBy = user.Email;
                    _context.Orders.Update(Order);
                    await _context.SaveChangesAsync();
                    return RedirectToPage();
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostDisplayAsync([FromBody] DataOrder data)
        {
            OrderDetails = await _context.OrderDetails.Where(x => x.ORD_ID == data.ID).ToListAsync();
            if (OrderDetails != null)
            {
                return new JsonResult(OrderDetails);
            }
            return NotFound();
        }

    }
    public class DataOrder
    {
        public int ID { get; set; }
    }
}
