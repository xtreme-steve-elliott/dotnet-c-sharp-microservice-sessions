using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Models;
using NotesApp.Repositories;

namespace NotesApp.Services
{
    public class NoteService : IModelService<Note>
    {
        private readonly IModelRepository<Note> _noteRepository;

        public NoteService(IModelRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public Task<IEnumerable<Note>> GetAllAsync()
        {
            return _noteRepository.GetAllAsync();
        }

        public Task<Note> GetByIdAsync(long id)
        {
            return _noteRepository.GetByIdAsync(id);
        }

        public Task<Note> AddAsync(Note toAdd)
        {
            return _noteRepository.AddAsync(toAdd);
        }

        public Task<bool> DeleteByIdAsync(long id)
        {
            return _noteRepository.DeleteByIdAsync(id);
        }
    }
}
