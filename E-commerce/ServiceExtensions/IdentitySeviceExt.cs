using System.Text;
using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace E_commerce.ServiceExtensions
{
    public static class IdentitySeviceExt
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(

                opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireNonAlphanumeric = true;
                    opt.Password.RequiredLength = 4;

                    opt.User.RequireUniqueEmail = true;
                }

                ).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            services.AddLogging();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;

        }
    }
}
