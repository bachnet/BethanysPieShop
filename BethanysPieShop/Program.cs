using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddDbContext<BethanysPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapDefaultControllerRoute();
DbInitializer.Seed(app);
app.Run();
