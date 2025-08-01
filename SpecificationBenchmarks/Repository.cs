using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace SpecificationBenchmarks;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(BenchmarkDbContext dbContext) : base(dbContext)
    {
    }
}