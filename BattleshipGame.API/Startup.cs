using System;
using BattleshipGame.API.Extenstions;
using BattleshipGame.BLL.Helpers;
using BattleshipGame.BLL.Hubs;
using BattleshipGame.DAL.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace BattleshipGame.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSQL(Configuration);
            services.AddControllers();
            services.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
            services.AddSignalR(opt => opt.EnableDetailedErrors = true);
            services.AddGameModules();
            services.AddServices();
            services.AddRepositories();
            services.AddJWTConfiguration(Configuration);
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(options =>
            {
                options.AllowAnyHeader()
                .WithOrigins("http://localhost:3000")
                .AllowCredentials();
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(u =>
            {
                u.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend");
                u.DocumentTitle = "Title Documentation";
                u.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<GameHub>("/gameHub");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
