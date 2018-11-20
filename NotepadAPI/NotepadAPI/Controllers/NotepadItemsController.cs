using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotepadAPI.Models;

namespace NotepadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotepadItemsController : ControllerBase
    {
        private readonly NotepadAPIContext _context;

        public NotepadItemsController(NotepadAPIContext context)
        {
            _context = context;
        }

        // GET: api/NotepadItems
        [HttpGet]
        public IEnumerable<NotepadItem> GetNotepadItem()
        {
            return _context.NotepadItem;
        }

        // GET: api/NotepadItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotepadItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notepadItem = await _context.NotepadItem.FindAsync(id);

            if (notepadItem == null)
            {
                return NotFound();
            }

            return Ok(notepadItem);
        }

        // PUT: api/NotepadItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotepadItem([FromRoute] int id, [FromBody] NotepadItem notepadItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notepadItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(notepadItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotepadItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NotepadItems
        [HttpPost]
        public async Task<IActionResult> PostNotepadItem([FromBody] NotepadItem notepadItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.NotepadItem.Add(notepadItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotepadItem", new { id = notepadItem.Id }, notepadItem);
        }

        // DELETE: api/NotepadItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotepadItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notepadItem = await _context.NotepadItem.FindAsync(id);
            if (notepadItem == null)
            {
                return NotFound();
            }

            _context.NotepadItem.Remove(notepadItem);
            await _context.SaveChangesAsync();

            return Ok(notepadItem);
        }

        private bool NotepadItemExists(int id)
        {
            return _context.NotepadItem.Any(e => e.Id == id);
        }
    }
}