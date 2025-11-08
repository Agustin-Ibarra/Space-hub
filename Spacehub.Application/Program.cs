using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Spacehub.Application.Repository;
// using SpaceHub.Application.Controllers.Hubs;
using SpaceHub.Application.Data;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Repository;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

string? stringConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
  options.LoginPath = "/login";
  options.ExpireTimeSpan = TimeSpan.FromDays(20);
  options.SlidingExpiration = true;
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  options.Events = new CookieAuthenticationEvents
  {
    OnRedirectToLogin = context =>
    {
      if (context.Request.Path.StartsWithSegments("/api"))
      {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
      }
      else
      {
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
      }
    }
  };
});


if (stringConnection != null)
{
  builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(stringConnection));
  builder.Services.AddScoped<IUserRepository, UserRepository>();
  builder.Services.AddScoped<IAstronomicalObjectRepository, AstronomicalObjectRepository>();
  builder.Services.AddScoped<IpostRepository, PostRepository>();
  builder.Services.AddScoped<IItemRespotory, ItemRepository>();
  builder.Services.AddScoped<ICartRepository, CartRepository>();
  builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
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

app.MapHub<NotifyHub>("/api/posts");

app.Run();
