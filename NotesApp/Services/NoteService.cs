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
            throw new System.NotImplementedException();
        }

        public Task<Note> AddAsync(Note toAdd)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
