using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
