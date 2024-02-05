using Microsoft.EntityFrameworkCore;

namespace Notes.Identity.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            
            services.AddScoped<AuthDbContext>(provider =>
                provider.GetService<AuthDbContext>());
            
            return services;
        }
    }
}
