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
    }
}
