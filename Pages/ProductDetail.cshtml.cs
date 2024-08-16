using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages
{
    [Authorize]
    public class ProductDetailModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        [BindProperty(SupportsGet = true)]
        public int? ProID { get; set; }
        public Product? Product { get; set; }
        public IList<Review>? Reviews { get; set; }
        public Cart? Cart { get; set; }
        public CartDetail? CartDetail { get; set; }
        public IList<CartDetail>? CartDetails { get; set; }
        public ProductDetailModel(AppDataContext context, UserManager<AuthUser> userManager) 
        {
            _userManager = userManager;
            _context = context;
            Product = new Product();
            Reviews = new List<Review>();
            Cart = new Cart();
            CartDetail = new CartDetail();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }
            Product = await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.PRO_ID == ProID);
            Reviews = await _context.Reviews.Where(x => x.PRO_ID == ProID).AsNoTracking().ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync([FromBody] DataProductDetail data)
        {
            ///GET USSER
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }


            ///NEW CART OR TAKE CART
            Cart = await _context.Carts.FirstOrDefaultAsync(x => x.USE_ID == user.Id);
            if (Cart == null)
            {
                Cart = new Cart
                {
                    USE_ID = user.Id,
                    Customer = user.Email,
                    Phone = user.PhoneNumber,
                    Address = user.Address,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Email!,
                };
                await _context.Carts.AddAsync(Cart);
                await _context.SaveChangesAsync();
                Cart = await _context.Carts.FirstOrDefaultAsync(x => x.USE_ID == user.Id);
            }
            if(Cart == null)
            {
                return NotFound();
            }

            ///NEW CARTDETAIL OR UPDATE
            CartDetail = await _context.CartDetails.FirstOrDefaultAsync(x => x.PRO_ID == data.PRO_ID && x.CAR_ID == Cart.CAR_ID);
            if(CartDetail == null)
            {
                CartDetail = new CartDetail
                {
                    CAR_ID = Cart!.CAR_ID,
                    PRO_ID = data.PRO_ID,
                    Quantity = data.Quantity,
                    Price = data.Price,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Email!,
                };
                await _context.CartDetails.AddAsync(CartDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                CartDetail.Quantity = data.Quantity;
                CartDetail.Price = data.Price;
                CartDetail.UpdatedDate = DateTime.Now;
                CartDetail.UpdatedBy = user.Email;
                _context.CartDetails.Update(CartDetail);
                await _context.SaveChangesAsync();
            }


            ///UPDATE CART
            CartDetails = await _context.CartDetails.Where(x => x.CAR_ID == Cart.CAR_ID).ToListAsync();
            decimal total = 0;
            int quant = 0;
            foreach (var cdt in CartDetails)
            {
                total += cdt.Price;
                quant += cdt.Quantity;
            }
            Cart!.TotalPrice = total;
            Cart.UpdatedDate = DateTime.Now;
            Cart.UpdatedBy = user.Email;
            _context.Carts.Update(Cart);
            await _context.SaveChangesAsync();

            return new JsonResult(new {quantity = quant});
        }
    }
    public class DataProductDetail {
        public int PRO_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
