using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using SpecificationBenchmarks.Data;

namespace SpecificationBenchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [GlobalSetup]
    public async Task Setup()
    {
        await BenchmarkDbContext.SeedAsync();
    }


    [Benchmark]
    public async Task<Store?> SimpleQuery()
    {
        await using var dbContext = new BenchmarkDbContext();
        return await dbContext.Stores
            .Where(x => x.Id > 0)
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name)
            .Include(x => x.Company)
            .ThenInclude(x => x.Country)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    [Benchmark]
    public async Task<Store?> WithSpecification()
    {
        await using var dbContext = new BenchmarkDbContext();
        var spec = new StoreSpecification();
        var store = await dbContext.Stores
            .WithSpecification(spec)
            .FirstOrDefaultAsync();
        return store;
    }

    [Benchmark]
    public async Task<Store?> WithRepositoryAndSpecification()
    {
        await using var dbContext = new BenchmarkDbContext();
        var repository = new Repository<Store>(dbContext);
        var spec = new StoreSpecification();
        var store = await repository.FirstOrDefaultAsync(spec);
        return store;
    }
}

public class StoreSpecification : Specification<Store>
{
    public StoreSpecification()
    {
        Query.Where(s => s.Id > 0);
        Query.OrderBy(s => s.Id)
            .ThenBy(s => s.Name);
        Query.Include(s => s.Company).ThenInclude(x => x.Country);
        Query.AsNoTracking();
    }
}