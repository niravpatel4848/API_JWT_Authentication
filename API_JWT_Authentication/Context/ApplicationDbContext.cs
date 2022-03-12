using API_JWT_Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace API_JWT_Authentication.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public  DbSet<Item> Items { get; set; }
    }
}
