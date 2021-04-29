using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Services
{
    public interface IModelService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
