using paf.api.Models;
using System.Linq.Expressions;

namespace paf.api.Interfaces
{
    public interface IGenericRepostiory<T> where T:BaseEntity
    {
        Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<int> InsertAsync(T entiiy);
        void update(T entiiy);
        void Delete(T entiiy);
        Task saveChangesAsync();
        Task GetFilteredAsync(object[] values);
    }
}
