using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BattleshipGame.DAL.Database;
using BattleshipGame.DAL.Repositories;
using BattleshipGame.DAL.Repositories.Interfaces;
using BattleshipGame.BLL.Services;
using BattleshipGame.BLL.Services.Interfaces;
using BattleshipGame.BLL.Game.GameLogic;
using BattleshipGame.BLL.Game.GameLogic.Interfaces;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using BattleshipGame.BLL.Hubs;
using BattleshipGame.BLL.Authentication;
using System.Threading.Tasks;

namespace BattleshipGame.API.Extenstions
{
    public static class ConfigureServicesExtension
    {
        public static void AddJWTConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            var key = "this_is_my_service_key";
            service.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(key));

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
        
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            service.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<IAuthService, AuthService>();
        }


        public static void AddGameModules(this IServiceCollection service)
        {
            service.AddSingleton<IGameAI, GameAI>();
            service.AddSingleton<IRoomManager, RoomManager>();
        }

        public static void AddSQL(this IServiceCollection service, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString));
        }


        public static void AddSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Backend",
                    Version = "v1"
                });


                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Auth",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
            });
        }
    }
}