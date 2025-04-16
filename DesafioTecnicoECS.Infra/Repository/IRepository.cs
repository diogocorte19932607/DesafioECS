using System.Linq.Expressions;

namespace DesafioTecnicoECS.Infra.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        T GetOne(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Delete(T entity);
        void Delete(object id);
        void Update(object id, T entity);
    }
}
