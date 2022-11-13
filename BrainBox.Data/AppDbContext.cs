using BrainBox.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrainBox.Data
{
    public class AppDbContext : IdentityDbContext<BrainBoxUser>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of RefreshTokens.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Carts.
        /// </summary>
        public DbSet<Cart> Carts { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of ProductCategories.
        /// </summary>
        public DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of CartProducts.
        /// </summary>
        public DbSet<CartProduct> CartProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureModels();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
        }
    }
}
