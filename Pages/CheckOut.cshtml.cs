using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Cart? Cart { get; set; }
        public Order? Order { get; set; }
        public OrderDetail? OrderDetail { get; set; }
        public IList<CartDetail>? CartDetails { get; set; } = new List<CartDetail>();
        [BindProperty]
        public DataOrder Or { get; set; } = new DataOrder();
        public CheckOutModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            Cart = new Cart();
            Order = new Order();
            OrderDetail = new OrderDetail();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Cart = await _context.Carts.Include(x => x.CartDetails).FirstOrDefaultAsync(x => x.USE_ID == user.Id);
                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Cart = await _context.Carts.Include(x => x.CartDetails).FirstOrDefaultAsync(x => x.USE_ID == user.Id);
                if (Cart != null && Cart.CartDetails != null)
                {
                    Order = new Order
                    {
                        USE_ID = user.Id,
                        Customer = Or.Name,
                        Phone = Or.Phone,
                        Address = Or.Address,
                        TotalPrice = Cart.TotalPrice,
                        PaymentMethod = "COD",
                        IsPaid = null,
                        Note = Or.Note,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Email!
                    };
                    await _context.Orders.AddAsync(Order);
                    await _context.SaveChangesAsync();
                    Order = await _context.Orders.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(x => x.USE_ID == user.Id);
                    if(Order != null)
                    {
                        foreach (var cdt in Cart.CartDetails)
                        {
                            OrderDetail = new OrderDetail
                            {
                                ORD_ID = Order.ORD_ID,
                                PRO_ID = cdt.PRO_ID,
                                Quantity = cdt.Quantity,
                                Price = cdt.Price,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user.Email!
                            };
                            await _context.OrderDetails.AddAsync(OrderDetail);
                            await _context.SaveChangesAsync();
                        }
                        CartDetails = await _context.CartDetails.Where(x => x.CAR_ID == Cart.CAR_ID).ToListAsync();
                        _context.CartDetails.RemoveRange(CartDetails);
                        _context.Carts.Remove(Cart);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Index");
                    }
                }
            }
            return NotFound();
        }
    }
    public class DataOrder
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Note { get; set; }
    }
}
