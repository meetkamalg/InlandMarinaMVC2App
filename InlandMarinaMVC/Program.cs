using InlandMarinaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme). //"Cookies"
    AddCookie(opt => opt.LoginPath = "/Account/Login"); //login page: Account controller, Login method

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<InlandMarinaContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("MarinaConnection")));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
