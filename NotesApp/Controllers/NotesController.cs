using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllAsync()
        {
            var result = new Note[]
            {
                new () { Id = 1L, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };
            
            return Ok(result);
        }
    }
}
