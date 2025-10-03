using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    // global cache defaults for MVC where applicable
});
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Enable HTTP response compression (Brotli + Gzip) for faster asset delivery
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
builder.Services.Configure<ResponseCompressionOptions>(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/javascript",
        "text/css",
        "image/svg+xml"
    });
});

builder.Services.AddDbContextPool<BethanysPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});
builder.Services.AddResponseCaching();

var app = builder.Build();

// Use detailed errors in Development; production gets exception handler + HSTS
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Compress responses (including static files) when supported by the client
app.UseResponseCompression();
app.UseResponseCaching();
// Serve static files with long-lived caching for optimal load times
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static assets aggressively; versioned URLs (asp-append-version) make this safe
        const string cacheControl = "public, max-age=31536000, immutable"; // 1 year
        ctx.Context.Response.Headers["Cache-Control"] = cacheControl;
    }
});
app.MapDefaultControllerRoute();
DbInitializer.Seed(app);
app.Run();
