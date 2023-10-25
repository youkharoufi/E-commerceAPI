using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace E_commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;


        public CartController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> getCart(string userId)
        {
            return await _context.cart.Where(crt => crt.UserId == userId).FirstOrDefaultAsync();
        }


        [HttpPost("assign-new-cart-to-user/{userId}")]
        public async Task<ActionResult<Cart>> createCart(string userId)
        {
            var existingCart = await _context.cart.Where(crt => crt.UserId == userId).FirstOrDefaultAsync();

            if (existingCart == null)
            {
                var cart = new Cart()
                {
                    UserId = userId
                };

                _context.cart.Add(cart);
                await _context.SaveChangesAsync();

                return Ok(cart);
            }

            return Ok(existingCart);
        }

        [HttpGet("items-count/{userId}/{cartId}")]
        public async Task<ActionResult<int>> itemsCount(string userId, int cartId)
        {
            var cartItems = await _context.cartItems.Where(crtItms => crtItms.UserId == userId && crtItms.CartId == cartId).ToListAsync();

            if (cartItems == null)
            {
                return NotFound("Cart items not found for this user Id");
            }

            return Ok(cartItems.Count());
        }

        [HttpGet("get-all-cart-items/{cartId}")]
        public async Task<ActionResult<List<CartItems>>> getAllCartItems(int cartId)
        {
            var cartItems = await _context.cartItems.Where(crtitm => crtitm.CartId == cartId).ToListAsync();

            return Ok(cartItems);
        }

        [HttpPost("change-quantity-of-a-product/{itemId}/{newQuantity}")]
        public async Task<ActionResult<Cart>> addQuantityOfProducts(int itemId, int newQuantity)
        {
            var cartItem = await _context.cartItems.Where(crtitm => crtitm.ItemId == itemId).FirstOrDefaultAsync();

            cartItem.ItemQuantity = newQuantity;
            await _context.SaveChangesAsync();

            return Ok(cartItem);
        }

        [HttpGet("get-cart-total/{cartId}")]
        public async Task<ActionResult<double>> GetCartTotal(int cartId)
        {
            var cart = await _context.cart.Include(c => c.cartItems).Where(c => c.CartId == cartId).FirstOrDefaultAsync();

            if (cart == null) return NotFound("No such cart in database");

            double total = 0;

            foreach(var item in cart.cartItems)
            {
                item.ItemTotal = item.ItemPrice * item.ItemQuantity;
                total += item.ItemTotal;
            }

            cart.TotalPrice = total;

            await _context.SaveChangesAsync();
            return Ok(total);
        }

        [HttpDelete("delete-cart-item/{itemId}")]
        public async Task<ActionResult<CartItems>> deleteCartItem(int itemId)
        {
            try
            {
                var item = await _context.cartItems.FindAsync(itemId);

                if (item == null) return NotFound("No such item in database");

                _context.Entry(item).State = EntityState.Deleted;
                _context.cartItems.Remove(item);

                await _context.SaveChangesAsync();

                return Ok(item);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
