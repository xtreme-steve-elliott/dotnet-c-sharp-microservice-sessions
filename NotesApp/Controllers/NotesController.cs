using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        public const string GetByIdRouteName = "GetNoteById";
        private readonly IModelService<Note> _noteService;

        public NotesController(IModelService<Note> noteService)
        {
            _noteService = noteService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllAsync()
        {
            var result = await _noteService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}", Name = GetByIdRouteName)]
        public async Task<ActionResult<Note>> GetByIdAsync(long id)
        {
            var foundNote = await _noteService.GetByIdAsync(id);
            if (foundNote == null)
            {
                return NotFound();
            }

            return Ok(foundNote);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> AddAsync([FromBody] Note toAdd)
        {
            if (toAdd == null)
            {
                return BadRequest();
            }

            var processedNote = await _noteService.AddAsync(toAdd);
            return CreatedAtRoute(GetByIdRouteName, new {id = processedNote.Id}, processedNote);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteByIdAsync(long id)
        {
            var result = await _noteService.DeleteByIdAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
