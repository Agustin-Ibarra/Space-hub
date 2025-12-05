using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spacehub.Application.Repository;
using SpaceHub.Application.Data;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Logs;
using SpaceHub.Application.Repository;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
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

if (connectionString != null)
{
  builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
  builder.Services.AddScoped<IUserRepository, UserRepository>();
  builder.Services.AddScoped<IAstronomicalObjectRepository, AstronomicalObjectRepository>();
  builder.Services.AddScoped<IpostRepository, PostRepository>();
  builder.Services.AddScoped<IItemRespotory, ItemRepository>();
  builder.Services.AddScoped<ICartRepository, CartRepository>();
  builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
  builder.Services.AddScoped<IChatRepository, ChatRepository>();
}
else
{
  throw new Exception("La cedena de conexion es null");
}

builder.Services.AddRateLimiter(options =>
{
  options.AddPolicy("fixedWindows", context => RateLimitPartition.GetFixedWindowLimiter(
    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown ip address",
    factory: Key => new FixedWindowRateLimiterOptions
    {
      PermitLimit = 60,
      Window = TimeSpan.FromSeconds(60),
      QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
      QueueLimit = 0
    }
  ));
});

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<RequestLoggin>();
app.UseExceptionHandler(error => // middleware para interceptar los errores no controlados de los controladores
{
  error.Run(async context =>
  {
    var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>(); // obtener la interfaz que contiene detalles del error
    var TypeException = exceptionHandler?.Error;

    if (TypeException is SqlException)
    {
      context.Response.StatusCode = 503;
      await context.Response.WriteAsJsonAsync(new { error = "Ocurrio un error en la base de datos" });
    }
    else
    {
      context.Response.StatusCode = 500;
      await Task.CompletedTask;
    }
  });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotifyHub>("/api/posts");

app.Run();
