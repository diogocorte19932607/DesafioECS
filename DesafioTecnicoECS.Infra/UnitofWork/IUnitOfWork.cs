using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Repository;

namespace DesafioTecnicoECS.Infra.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;

        ClientContext Context { get; }
        int Save();
        Task<int> SaveAsync();
    }

}
