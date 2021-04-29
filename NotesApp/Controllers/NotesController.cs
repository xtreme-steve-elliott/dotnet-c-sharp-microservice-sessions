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
    }
}
