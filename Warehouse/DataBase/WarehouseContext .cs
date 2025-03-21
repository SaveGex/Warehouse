using Microsoft.EntityFrameworkCore;
using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase.Models;

namespace Warehouse.DataBase
{
    public class WarehouseContext : DbContext
    {
        public DbSet<BaseElement> Indefined { get; set; }


        public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseElement>().ToTable("Undefined");

            modelBuilder.Entity<BaseElement>()
                .HasKey(e => e.Id);
        }

    }
}
