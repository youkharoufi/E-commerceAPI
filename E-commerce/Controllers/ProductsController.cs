using System.Net.WebSockets;
using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult<List<Product>>> getAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("get-one-product/{productId}")]
        public async Task<ActionResult<Product>> getOneProduct(int productId)
        {
            var product = await _context.Products.Where(prdct=> prdct.ProductId == productId).FirstOrDefaultAsync();

            return Ok(product);
        }

        [HttpPost("add-to-cart/{productId}/{userId}/{quantity}")]
        public async Task<ActionResult<Cart>> addToCart(int productId, string userId, int quantity)
        {
            var cart = await _context.cart.Where(crt => crt.UserId == userId).FirstOrDefaultAsync();
            var product = await _context.Products.Where(prd => prd.ProductId == productId).FirstOrDefaultAsync();

            if(cart == null)
            {
                return BadRequest("Create your cart first");
            }

            if(product == null)
            {
                return NotFound("No such product in database");
            }

            if (cart.cartItems != null) cart.cartItems = new List<CartItems>();

            var cartItem = new CartItems
            {
                UserId = userId,
                ItemName = product.ProductName,
                CartId = cart.CartId,
                Description = product.Description,
                ItemPhotoUrl = product.PhotoUrl1,
                ItemPrice = product.Price,
                ItemQuantity = quantity
            };

            var cartIn = await _context.cartItems.Where(crtItms => crtItms.ItemName == product.ProductName).FirstOrDefaultAsync();
            if(cartIn != null)
            {
                cartIn.ItemQuantity = cartIn.ItemQuantity + quantity;
                await _context.SaveChangesAsync();
                return Ok(cart);
            }

            cart.cartItems.Add(cartItem);

            try
            {
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return Ok(cart);
        }

    }
}
