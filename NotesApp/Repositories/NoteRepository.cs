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
    }
}
