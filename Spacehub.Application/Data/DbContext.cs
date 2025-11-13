using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<Chat> Chats { get; set; }
  public DbSet<ChatMessage> ChatMessages { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Item> Items { get; set; }
  public DbSet<Cart> Carts { get; set; }
  public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
  public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<AstronomicalObject> AstronomicalObjects { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().ToTable("users");
    modelBuilder.Entity<Role>().ToTable("roles");
    modelBuilder.Entity<Chat>().ToTable("chats");
    modelBuilder.Entity<ChatMessage>().ToTable("chats_messages");
    modelBuilder.Entity<Category>().ToTable("categories");
    modelBuilder.Entity<Item>().ToTable("items");
    modelBuilder.Entity<Cart>().ToTable("carts");
    modelBuilder.Entity<PurchaseOrder>().ToTable("purchase_orders");
    modelBuilder.Entity<PurchaseDetail>().ToTable("purchase_details");
    modelBuilder.Entity<Post>().ToTable("posts");
    modelBuilder.Entity<AstronomicalObject>().ToTable("astronomical_objects");
  }
}