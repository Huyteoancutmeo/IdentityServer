using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }
            var builder = WebApplication.CreateBuilder(args);

            if (seed)
            {
                SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("MyDB"));
            }

            var assembly = typeof(Program).Assembly.GetName().Name;

            builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("MyDB"), 
                    opt => opt.MigrationsAssembly(assembly)));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>();
            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                    b.UseMySQL(builder.Configuration.GetConnectionString("MyDB"), 
                        opt => opt.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                    b.UseMySQL(builder.Configuration.GetConnectionString("MyDB"),
                        opt => opt.MigrationsAssembly(assembly));
                })
                .AddDeveloperSigningCredential();

            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}