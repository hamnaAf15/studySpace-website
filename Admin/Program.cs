using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using studySpaceWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? Add MVC Controllers with Views
builder.Services.AddControllersWithViews();

// ?? Register your database context using SQL Server
builder.Services.AddDbContext<StudySpaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ?? Add session support for login/logout (admin session, etc.)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // optional timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ?? Add CORS for API use (optional if using APIs with React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// ?? Error handling for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // serve static files (CSS, JS, images)

// ?? Middleware setup
app.UseRouting();
app.UseCors("AllowAll"); // Enable CORS policy
app.UseAuthorization();
app.UseSession(); // Enable session middleware

// ?? Set Login as the default page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
