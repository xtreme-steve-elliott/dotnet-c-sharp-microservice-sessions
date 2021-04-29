using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Repositories
{
    public interface IModelRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T toAdd);
        Task<bool> DeleteByIdAsync(long id);
    }
}
