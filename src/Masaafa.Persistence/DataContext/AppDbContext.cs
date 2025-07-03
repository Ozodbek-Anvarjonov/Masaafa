using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Masaafa.Persistence.DataContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    // First-tier
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();

    public DbSet<TransferRequest> TransferRequests => Set<TransferRequest>();
    public DbSet<TransferRequestItem> TransferRequestItems => Set<TransferRequestItem>();

    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public DbSet<Payment> Payments => Set<Payment>();

    // Second-tier
    public DbSet<ItemGroup> ItemGroups => Set<ItemGroup>();
    public DbSet<Item> Items => Set<Item>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseItem> WarehouseItems => Set<WarehouseItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}