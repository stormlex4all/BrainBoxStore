using BrainBox.Web.Controllers.Handlers;
using BrainBox.Web.Controllers.Handlers.Contracts;
using BrainBox.Repositories;
using BrainBox.Repositories.Contracts;
using BrainBox.Core.CoreServices.Contracts;
using BrainBox.Core.CoreServices;

namespace BrainBox.API.DependencyInjections
{
    public static class AllCollectionExtensions
    {
        /// <summary>
        /// Configuring dependencies for refresh token
        /// </summary>
        public static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //register handlers
            services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();
            services.AddScoped<ICartHandler, CartHandler>();
            services.AddScoped<IProductHandler, ProductHandler>();
            services.AddScoped<ICartProductHandler, CartProductHandler>();
            services.AddScoped<IProductCategoryHandler, ProductCategoryHandler>();

            //register repositories
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartProductRepository, CartProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IBrainBoxUserRepository, BrainBoxUserRepository>();

            //register core services
            services.AddScoped(typeof(ICoreValidationService<>), typeof(CoreValidationService<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}
