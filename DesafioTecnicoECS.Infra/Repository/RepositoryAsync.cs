using Microsoft.EntityFrameworkCore;

using DesafioTecnicoECS.Infra.UnitofWork;

namespace DesafioTecnicoECS.Infra.Repository
{

    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly IUnitofWork _unitOfWork;
        public RepositoryAsync(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate, string includeProperties)
        {

            var query = _unitOfWork.Context.Set<T>().Where(predicate);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _unitOfWork.Context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _unitOfWork.Context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }
        public async Task Insert(T entity)
        {
            if (entity != null)
                await _unitOfWork.Context.Set<T>().AddAsync(entity);
        }
        public async Task Update(object id, T entity)
        {
            if (entity != null)
            {
                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            }
        }
        public async Task Delete(object id)
        {
            T entity = await _unitOfWork.Context.Set<T>().FindAsync(id);
            Delete(entity);
        }
        public void Delete(T entity)
        {
            if (entity != null) _unitOfWork.Context.Set<T>().Remove(entity);
        }
    }
}
