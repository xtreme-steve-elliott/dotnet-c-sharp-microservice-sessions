using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Services
{
    public interface IModelService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T toAdd);
        Task<bool> DeleteByIdAsync(long id);
    }
}
