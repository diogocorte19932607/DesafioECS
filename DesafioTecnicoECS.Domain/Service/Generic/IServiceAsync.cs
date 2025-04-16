using System.Linq.Expressions;

namespace DesafioTecnicoECS.Domain.Service.Generic
{
    public interface IServiceAsync<Tv, Te>
    {
        Task<IEnumerable<Tv>> GetAll();
        Task<Guid> Add(Tv obj);
        Task<int> Update(Tv obj);
        Task<int> Remove(Guid id);
        Task<Tv> GetOne(Guid id);
        Task<IEnumerable<Tv>> Get(Expression<Func<Te, bool>> predicate);
    }
}
