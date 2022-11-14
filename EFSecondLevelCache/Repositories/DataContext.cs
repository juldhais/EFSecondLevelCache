using EFSecondLevelCache.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFSecondLevelCache.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
}