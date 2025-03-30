using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using SuppliersApi.Models;
namespace SuppliersApi.Context;

public class SupplierDbContext : DbContext
{
  public DbSet<Supplier> Suppliers { get; set; }
  public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Supplier>().ToCollection("suppliers");
  }
}