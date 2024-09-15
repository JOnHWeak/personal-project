using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personal_project.Models;
using personal_project.Helper;
using static personal_project.DTO.TitleDTO;

namespace personal_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly pubsContext _context;

        public TitleController(pubsContext context)
        {
            _context = context;
        }

        // GET: api/titles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<title>>> GetTitles()
        {
            var titles = await _context.titles.ToListAsync();
            if (!titles.Any())
            {
                return NoContent();
            }
            return Ok(titles);
        }

        // GET: api/titles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<title>> GetTitle(string id)
        {
            var title = await _context.titles.FindAsync(id);

            if (title == null)
            {
                return NotFound(new { Message = $"Title with ID '{id}' not found." });
            }

            return Ok(title);
        }

        // PUT: api/titles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitle(string id, UpdateTitle form)
        {
            //Check if title_id existing
            var titleToUpdate = await _context.titles.FindAsync(id);
            if (titleToUpdate == null)
            {
                return NotFound(new { Message = $"Title with ID '{id}' not found." });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           
            // Check if publisher exists
            if (!await CheckIDExisted.PublisherExists(_context, form.pub_id))
            {
                return BadRequest(new { Message = "The specified Publisher ID does not exist." });
            }           

            // Update title properties
            titleToUpdate.title1 = form.title;
            titleToUpdate.type = string.IsNullOrWhiteSpace(form.type) ? "UNDECIDED" : form.type;
            titleToUpdate.pub_id = form.pub_id;
            titleToUpdate.price = form.price;
            titleToUpdate.advance = form.advance;
            titleToUpdate.royalty = form.royalty;
            titleToUpdate.ytd_sales = form.ytd_sales;
            titleToUpdate.notes = form.notes;
            titleToUpdate.pubdate = form.pubdate;

            _context.Entry(titleToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Title updated successfully." });
            }           
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while updating the title.", Details = ex.Message });
            }
        }

        // POST: api/titles
        [HttpPost]
        public async Task<ActionResult<title>> CreateTitle(TitleForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if title_id already exists
            if (await CheckIDExisted.TitleExists(_context, form.title_id))
            {
                return Conflict(new { Message = "Title with the same ID already exists." });
            }

            // Check if pub_id exists
            if (!await CheckIDExisted.PublisherExists(_context, form.pub_id))
            {
                return BadRequest(new { Message = "The specified Publisher ID does not exist." });
            }
            //Create title properties
            var title = new title
            {
                title_id = form.title_id,
                title1 = form.title,
                type = string.IsNullOrWhiteSpace(form.type) ? "UNDECIDED" : form.type,
                pub_id = form.pub_id,
                price = form.price,
                advance = form.advance,
                royalty = form.royalty,
                ytd_sales = form.ytd_sales,
                notes = form.notes,
                pubdate = form.pubdate
            };

            _context.titles.Add(title);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTitle), new { id = title.title_id }, new { Message = "Title created successfully.", title });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while updating the title.", Details = ex.Message });
            }     
        }
    

        // DELETE: api/titles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitle(string id)
        {
            var title = await _context.titles.FindAsync(id);
            if (title == null)
            {
                return NotFound(new { Message = $"Title with ID '{id}' not found." });
            }

            _context.titles.Remove(title);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Title deleted successfully." });
        }
    }
}
