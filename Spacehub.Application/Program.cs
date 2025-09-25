using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Repository;

var builder = WebApplication.CreateBuilder(args);

string? stringConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

if (stringConnection != null)
{
  builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(stringConnection));
  builder.Services.AddScoped<IUserRepository, UserRepository>();
}
else
{
  throw new Exception("La cedena de conexion es null");
}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
