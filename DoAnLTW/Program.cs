using DoAnLTW.Data;
using DoAnLTW.Models;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { CategoryName = "Bánh & Kẹo", DanhmucMota = "Các loại bánh quy, snack, kẹo ngọt" },
            new Category { CategoryName = "Sữa & Chế phẩm từ sữa", DanhmucMota = "Sữa tươi, sữa chua, phô mai" },
            new Category { CategoryName = "Thịt & Hải sản tươi sống", DanhmucMota = "Thịt heo, thịt bò, cá, tôm tươi" },
            new Category { CategoryName = "Cơm & Đồ ăn chế biến sẵn", DanhmucMota = "Cơm hộp, đồ ăn liền tiện lợi" },
            new Category { CategoryName = "Rau củ & Trái cây", DanhmucMota = "Rau xanh, củ quả tươi sạch" },
            new Category { CategoryName = "Thực phẩm đông lạnh", DanhmucMota = "Xúc xích, cá viên, há cảo" }
        );
        context.SaveChanges();
    }

    if (!context.Accounts.Any())
    {
        context.Accounts.AddRange(
            new Account { Username = "admin", Password = "123456", FullName = "Quản Trị Viên", Phone = "0909123456", Address = "140 Lê Trọng Tấn, Tân Phú", Role = "Admin" },
            new Account { Username = "khachhang1", Password = "123456", FullName = "Nguyễn Văn An", Phone = "0909123123", Address = "Tân Phú, TP.HCM", Role = "User" }
        );
        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        var firstCatId = context.Categories.First().CategoryId;
        context.Products.AddRange(
            new Product { ProductName = "Bánh quy bơ Danisa 454g", Price = 135000, ImageUrl = "banh-danisa.jpg", ProductMota = "Bánh quy bơ thơm ngon", SoLuongTon = 50, CategoryId = firstCatId },
            new Product { ProductName = "Snack khoai tây Lay's 95g", Price = 22000, ImageUrl = "snack-lays.jpg", ProductMota = "Khoai tây chiên giòn rụm", SoLuongTon = 100, CategoryId = firstCatId }
        );
        context.SaveChanges();
    }
}
app.Run();
