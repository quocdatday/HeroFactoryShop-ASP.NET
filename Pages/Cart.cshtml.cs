using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly AppDataContext _context;
        public Cart? Cart { get; set; }
        public IList<CartDetail>? CartDetails { get; set; }
        public CartDetail? CartDetail { get; set; }
        public CartModel(AppDataContext context, UserManager<AuthUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            Cart = new Cart();
            CartDetails = new List<CartDetail>();
            CartDetail = new CartDetail();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            Cart = await _context.Carts.FirstOrDefaultAsync(x => x.USE_ID == user.Id);
            if (Cart != null)
            {
                CartDetails = await _context.CartDetails.Include(x => x.Product).Where(x => x.CAR_ID == Cart.CAR_ID).ToListAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDltCdtAsync([FromBody] DataCart data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            CartDetail = await _context.CartDetails.FirstOrDefaultAsync(x => x.CARD_ID == data.CARD_ID);
            if (CartDetail == null)
            {
                return Page();
            }
            _context.CartDetails.Remove(CartDetail);
            await _context.SaveChangesAsync();
            //
            var x = 1;
            Cart = await _context.Carts.FirstOrDefaultAsync(x => x.USE_ID == user.Id);
            if (Cart == null) { return NotFound(); }
            CartDetails = await _context.CartDetails.Where(x => x.CAR_ID == Cart.CAR_ID).ToListAsync();
            if(CartDetails == null || !CartDetails.Any())
            {
                x = 0;
            }
            //
            return new JsonResult(new 
            {
                items = CartDetail.Quantity,
                prices = (int)CartDetail.Price,
                check = x,
            });
        }


        public async Task<IActionResult> OnPostUpCdtAsync([FromBody] DataCart data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null){ return NotFound(); }
            Cart = await _context.Carts.FirstOrDefaultAsync(x => x.USE_ID == user.Id);
            if(Cart == null) { return NotFound(); }
            CartDetail = await _context.CartDetails.Include(x => x.Product).FirstOrDefaultAsync(x => x.CARD_ID == data.CARD_ID);
            if(CartDetail == null) 
            { 
                return NotFound();
            }
            else
            {
                var quant = CartDetail.Quantity + data.Num;
                var price = quant * CartDetail.Product!.Price;
                CartDetail.Quantity = quant;
                CartDetail.Price = price;
                CartDetail.UpdatedDate = DateTime.Now;
                CartDetail.UpdatedBy = user.Email;
                _context.CartDetails.Update(CartDetail);
                await _context.SaveChangesAsync();
            }
            CartDetails = await _context.CartDetails.Where(x => x.CAR_ID == Cart.CAR_ID).ToListAsync();
            if (CartDetails == null) { return NotFound(); }
            var itms = 0;
            var ttl = 0;
            foreach (var cdt in CartDetails)
            {
                itms += cdt.Quantity;
                ttl += (int)cdt.Price;
            }

            return new JsonResult(new
            {
                items = itms,
                totals = ttl,
                quantity = CartDetail.Quantity,
                price = CartDetail.Price
            });
        }
    }
    public class DataCart
    {
        public int CARD_ID { get; set; }
        public int Num { get; set; }
    }
}
