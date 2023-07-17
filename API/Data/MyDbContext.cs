using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) :
        base(options)
        { }

        public DbSet<Shop> Shops { get; set; }
    }
}
