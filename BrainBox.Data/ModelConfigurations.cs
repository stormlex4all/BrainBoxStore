using BrainBox.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBox.Data
{
    public static class ModelConfigurations
    {

        public static void ConfigureModels(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.Property(r => r.UserEmail).HasMaxLength(50);
                b.Property(r => r.Token).HasMaxLength(128);
                b.Property(r => r.JwtId).HasMaxLength(128);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(r => r.Price)
                .HasColumnType("decimal(18,2)");
            });
        }
    }
}
