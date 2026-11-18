using DoAnLTW.Data;
using DoAnLTW.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnLTW.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var acc = _context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (acc == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

            HttpContext.Session.SetInt32("AccountId", acc.AccountId);
            HttpContext.Session.SetString("FullName", acc.FullName ?? acc.Username);
            HttpContext.Session.SetString("Role", acc.Role ?? "User");

            if (acc.Role == "Admin")
            {
                return RedirectToAction("Index", "Order", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account account)
        {
            ModelState.Remove("Role");

            if (_context.Accounts.Any(a => a.Username == account.Username))
            {
                ViewBag.Error = "Tên đăng nhập này đã tồn tại!";
                return View(account);
            }

            if (ModelState.IsValid)
            {
                account.Role = "User"; 
                _context.Accounts.Add(account);
                _context.SaveChanges();

                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(account);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}