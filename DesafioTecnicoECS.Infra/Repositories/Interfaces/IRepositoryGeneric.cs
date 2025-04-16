using System.Linq.Expressions;

namespace DesafioTecnicoECS.Infra.Repositories.Interfaces
{

    public interface IRepositoryGeneric<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        int Count();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        int Save();
    }
}
