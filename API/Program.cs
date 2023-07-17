using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectinString = builder.Configuration.GetConnectionString("MyDB");
            // Add DbContext
            builder.Services.AddDbContext<MyDbContext>(options => options.UseMySQL(connectinString));

            builder.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = "https://localhost:5443";
                    options.ApiName = "ShopAPI";
                });
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}