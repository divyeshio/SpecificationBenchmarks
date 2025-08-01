using Microsoft.EntityFrameworkCore;
using SpecificationBenchmarks.Data;

namespace SpecificationBenchmarks;

public class BenchmarkDbContext : DbContext
{
    public DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=benchmark.db");
    }

    public static async Task SeedAsync()
    {
        using var context = new BenchmarkDbContext();
        var created = await context.Database.EnsureCreatedAsync();

        if (!created) return;

        var company = new Company
        {
            Name = "Company 1",
            Country = new Country
            {
                Name = "Country 1"
            }
        };
        var store1 = new Store
        {
            Name = "Store 1",
            Company = company,
            Products =
            [
                new Product { Name = "Product 1" }
            ]
        };
        var store2 = new Store
        {
            Name = "Store 2",
            Company = company,
            Products =
            [
                new Product { Name = "Product 2" }
            ]
        };

        context.AddRange(store1, store2);
        await context.SaveChangesAsync();
    }
}