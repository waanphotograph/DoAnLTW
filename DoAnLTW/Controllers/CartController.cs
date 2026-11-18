using DoAnLTW.Data;
using DoAnLTW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DoAnLTW.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CART_KEY = "MY_CART";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CART_KEY);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CART_KEY, JsonSerializer.Serialize(cart));
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                if (quantity > 0) item.Quantity = quantity;
                else cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(string shippingAddress, string orderPhone)
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index");

            int accountId = HttpContext.Session.GetInt32("AccountId") ?? 2;

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(c => c.Total),
                Status = "Chờ duyệt",
                ShippingAddress = shippingAddress,
                OrderPhone = orderPhone,
                AccountId = accountId
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); 

            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    SoLuong = item.Quantity,
                    DonGia = item.Price
                };
                _context.OrderDetails.Add(detail);
            }

            _context.SaveChanges();
            HttpContext.Session.Remove(CART_KEY); 

            return View("OrderSuccess", order.OrderId);
        }
    }
}