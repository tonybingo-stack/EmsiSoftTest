using Microsoft.EntityFrameworkCore;
using EmsisoftTest.Models;

namespace EmsisoftTest.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options)
    : base(options)
        {

        }

        public DbSet<HashData> hashes { get; set; }
    }
}
