using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Shop
{
    public partial class ShopDbContext : DbContext
    {
        public ShopDbContext()
            : base("name=Shop")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Устанавливаем отношение "один ко многим" между заказами и продуктами
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("OrderProducts");
                    m.MapLeftKey("OrderId");
                    m.MapRightKey("ProductId");
                });

            // Устанавливаем связь "один к одному" между заказами и клиентами
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId);
        }
    }
}
