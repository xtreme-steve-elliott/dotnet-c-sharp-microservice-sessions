using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Repositories
{
    public interface IModelRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
