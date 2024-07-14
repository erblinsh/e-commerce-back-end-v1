using System.Collections;
using Application;

namespace Presistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly APIDbContext _dbContext;

    private Hashtable _repositories;

    public UnitOfWork(APIDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Complete()
    {
        var numberOfAffectedRows = _dbContext.SaveChanges();
        return numberOfAffectedRows > 0;
    }
    
    public async Task<bool> CompleteAsync()
    {
        var numberOfAffectedRows = await _dbContext.SaveChangesAsync();
        return numberOfAffectedRows > 0;
    }

    public ILifeEccommerceRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.Contains(type))
        {
            var repositoryType = typeof(LifeEccommerceRepository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (ILifeEccommerceRepository<TEntity>)_repositories[type];
    }
}