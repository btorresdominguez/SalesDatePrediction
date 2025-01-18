using Microsoft.EntityFrameworkCore;
using SalesDatePrediction.Models;

namespace SalesDatePrediction.Interfaces.RepositoryPattern
{
    public class SalesDatePredictionDbContext : DbContext
    {
        // Constructor para pasar las opciones del DbContext
        public SalesDatePredictionDbContext(DbContextOptions<SalesDatePredictionDbContext> options)
            : base(options)
        {
        }

        // Propiedades DbSet para las entidades
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CustomerOrderPrediction> CustomerOrderPredictions { get; set; }

        public DbSet<CustomerOrderDto> CustomerOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla Customers
            modelBuilder.Entity<Customer>()
                .ToTable("Customers", "Sales");

            // Configuración de la tabla Orders
            modelBuilder.Entity<OrderEntity>()
                .ToTable("Orders", "Sales");

            // Configuración para CustomerOrderPrediction
            modelBuilder.Entity<CustomerOrderPrediction>()
                .HasNoKey();

            modelBuilder.Entity<CustomerOrderDto>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<EmployeeDto>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<ShipperDto>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<ProductDto>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("Orders", "Sales");

                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.Freight).HasColumnType("money");
                entity.Property(o => o.ShipName).HasMaxLength(40).IsRequired();
                entity.Property(o => o.ShipAddress).HasMaxLength(60).IsRequired();
                entity.Property(o => o.ShipCity).HasMaxLength(15).IsRequired();
                entity.Property(o => o.ShipRegion).HasMaxLength(15);
                entity.Property(o => o.ShipPostalCode).HasMaxLength(10);
                entity.Property(o => o.ShipCountry).HasMaxLength(15).IsRequired();

                // Relación con OrderDetails
                entity.HasMany(o => o.OrderDetails)
                      .WithOne(od => od.Order)
                      .HasForeignKey(od => od.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de OrderDetails
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails", "Sales");

                entity.HasKey(od => new { od.OrderId, od.ProductId });

                entity.Property(od => od.UnitPrice).HasColumnType("money").IsRequired();
                entity.Property(od => od.Qty).IsRequired();
                entity.Property(od => od.Discount).HasColumnType("numeric(4,3)").IsRequired();
            });

            modelBuilder.Entity<Product>()
        .ToTable("Products", "Production");


        }
    }
}