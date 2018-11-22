using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactBookAPI.Models;

namespace ContactBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactItemsController : ControllerBase
    {
        private readonly ContactBookAPIContext _context;

        public ContactItemsController(ContactBookAPIContext context)
        {
            _context = context;
        }

        // GET: api/ContactItems
        [HttpGet]
        public IEnumerable<ContactItem> GetContactItem()
        {
            return _context.ContactItem;
        }

        // GET: api/ContactItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactItem = await _context.ContactItem.FindAsync(id);

            if (contactItem == null)
            {
                return NotFound();
            }

            return Ok(contactItem);
        }

        // PUT: api/ContactItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactItem([FromRoute] int id, [FromBody] ContactItem contactItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactItemExists(id))
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

        // POST: api/ContactItems
        [HttpPost]
        public async Task<IActionResult> PostContactItem([FromBody] ContactItem contactItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactItem.Add(contactItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactItem", new { id = contactItem.Id }, contactItem);
        }

        // DELETE: api/ContactItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactItem = await _context.ContactItem.FindAsync(id);
            if (contactItem == null)
            {
                return NotFound();
            }

            _context.ContactItem.Remove(contactItem);
            await _context.SaveChangesAsync();

            return Ok(contactItem);
        }

        private bool ContactItemExists(int id)
        {
            return _context.ContactItem.Any(e => e.Id == id);
        }
    }
}