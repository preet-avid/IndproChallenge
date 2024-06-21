using Indpro.API.Data.Entity.DbSet;
using Microsoft.EntityFrameworkCore;

namespace Indpro.API.Data.Entity;
public class IndproChallengeDbContext: DbContext
{
    public IndproChallengeDbContext(DbContextOptions<IndproChallengeDbContext> options)
          : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}

