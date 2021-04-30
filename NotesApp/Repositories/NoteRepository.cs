using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public class NoteRepository : IModelRepository<Note>
    {
        private readonly NotesAppDbContext _dbContext;

        public NoteRepository(NotesAppDbContext dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.Database.IsInMemory())
            {
                _dbContext.Database.EnsureCreated();
            }
        }

        public Task<IEnumerable<Note>> GetAllAsync()
        {
            return Task.FromResult(_dbContext.Set<Note>().AsEnumerable() ?? Enumerable.Empty<Note>());
        }

        public Task<Note> GetByIdAsync(long id)
        {
            return _dbContext.Set<Note>().FindAsync(id).AsTask();
        }

        public async Task<Note> AddAsync(Note toAdd)
        {
            var entry = await _dbContext.Set<Note>().AddAsync(toAdd);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<bool> DeleteByIdAsync(long id)
        {
            var notes = _dbContext.Set<Note>();
            var foundNote = await notes.FindAsync(id);
            if (foundNote == null)
            {
                return false;
            }

            notes.Remove(foundNote);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
