using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personal_project.Helper;
using personal_project.Models;
using static personal_project.DTO.StoreDTO;

namespace personal_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly pubsContext _context;

        public StoresController(pubsContext context)
        {
            _context = context;
        }

      

        // GET: api/stores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<store>>> GetStores()
        {
            var stores = await _context.stores.ToListAsync();
            if (!stores.Any())
            {
                return NoContent();
            }
            return Ok(stores);
        }

        // GET: api/stores/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<store>> GetStore(string id)
        {
            var store = await _context.stores.FindAsync(id);

            if (store == null)
            {
                return NotFound(new { Message = $"Store with ID '{id}' not found." });
            }

            return Ok(store);
        }

        // PUT: api/stores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(string id, UpdateStore storeForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = await _context.stores.FindAsync(id);
            if (store == null)
            {
                return NotFound(new { Message = $"Store with ID '{id}' not found." });
            }

            // Update the store's details
            store.stor_name = storeForm.stor_name;
            store.stor_address = storeForm.stor_address;
            store.city = storeForm.city;
            store.state = storeForm.state;
            store.zip = storeForm.zip;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Store updated successfully." });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("CONSTRAINT"))
                {
                    return BadRequest(new { Message = "Data integrity violation. Please check the values for StoreID, state, and zip." });
                }
                throw;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while updating the store.", Details = ex.Message });
            }
        }

        // POST: api/stores
        [HttpPost]
        public async Task<ActionResult<store>> CreateStore(StoreForm storeForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the StoreID already exists
            var existingStore = await _context.stores.AnyAsync(s => s.stor_id == storeForm.stor_id);
            if (existingStore)
            {
                return BadRequest(new { Message = "The StoreID already exists." });
            }
            try
            {
                var store = new store
                {
                    stor_id = storeForm.stor_id,
                    stor_name = storeForm.stor_name,
                    stor_address = storeForm.stor_address,
                    city = storeForm.city,
                    state = storeForm.state,
                    zip = storeForm.zip
                };

                _context.stores.Add(store);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStore), new { id = store.stor_id }, new { Message = "Store created successfully.", store });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("CONSTRAINT"))
                {
                    return BadRequest(new { Message = "Data integrity violation. Please check the values for StoreID, state, and zip." });
                }
                throw;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while creating the store.", Details = ex.Message });
            }
        }

        // DELETE: api/stores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(string id)
        {
            var store = await _context.stores.FindAsync(id);
            if (store == null)
            {
                return NotFound(new { Message = $"Store with ID '{id}' not found." });
            }

            _context.stores.Remove(store);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Store '{store.stor_name}' with ID '{store.stor_id}' deleted successfully." });
        }

        private bool StoreExists(string id)
        {
            return _context.stores.Any(e => e.stor_id == id);
        }
    }
}
