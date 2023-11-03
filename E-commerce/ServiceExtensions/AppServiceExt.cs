using E_commerce.Data;
using E_commerce.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace E_commerce.ServiceExtensions
{
    public static class AppServiceExt
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });


            services.AddScoped<ITokenService, TokenService>();

            services.AddCors();

            return services;
        }
    }
}
