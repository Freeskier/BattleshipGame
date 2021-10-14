using BattleshipGame.DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BattleshipGame.API.Extenstions
{
    public static class ConfigureServicesExtension
    {
        public static void AddJWTConfiguration(IServiceCollection service, IConfiguration configuration)
        {
            var key = "this_is_my_service_key";

            service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }
            );
        }
        
        public static void AddRepositories(IServiceCollection service)
        {

        }

        public static void AddServices(IServiceCollection service)
        {

        }

        public static void AddSignalR(IServiceCollection service)
        {

        }

        public static void AddSQL(IServiceCollection service, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString));
        }

        public static void AddSwagger(IServiceCollection service)
        {

        }
    }
}