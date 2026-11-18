using DoAnLTW.Data;
using DoAnLTW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnLTW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId, string? search)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.Search = search;

            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search));
            }

            return View(products.ToList());
        }
        public IActionResult Products(string? category, string? q, string? sort)
        {
            ViewBag.Categories = _context.Categories.ToList();

            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category != null &&
                    EF.Functions.ILike(p.Category.CategoryName, $"%{category}%"));
            }

            if (!string.IsNullOrEmpty(q))
            {
                products = products.Where(p => EF.Functions.ILike(p.ProductName, $"%{q}%"));
            }

            products = sort switch
            {
                "low" => products.OrderBy(p => p.Price),
                "high" => products.OrderByDescending(p => p.Price),
                _ => products.OrderByDescending(p => p.ProductId)
            };

            return View(products.ToList());
        }
    }
}