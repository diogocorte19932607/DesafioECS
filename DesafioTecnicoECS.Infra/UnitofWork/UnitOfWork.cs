using Microsoft.EntityFrameworkCore;

using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Repository;

namespace DesafioTecnicoECS.Infra.UnitofWork
{

    public class UnitOfWork : IUnitofWork
    {
        public ClientContext Context { get; }

        private Dictionary<Type, object> _repositoriesAsync;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(ClientContext context)
        {
            Context = context;
            _disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(this);
            return (IRepository<TEntity>)_repositories[type];
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositoriesAsync = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositoriesAsync.ContainsKey(type)) _repositoriesAsync[type] = new RepositoryAsync<TEntity>(this);
            return (IRepositoryAsync<TEntity>)_repositoriesAsync[type];
        }

        public int Save()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }
        }
        public async Task<int> SaveAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return -1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }
    }

}
