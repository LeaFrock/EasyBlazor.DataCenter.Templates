using Microsoft.EntityFrameworkCore;
using AppName.DataCenter.Server.Data.Models;

#nullable disable

namespace AppName.DataCenter.Server.Data
{
    public partial class AppNameDbContext : DbContext
    {
        public AppNameDbContext()
        {
        }

        public AppNameDbContext(DbContextOptions<AppNameDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}