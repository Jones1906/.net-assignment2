using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;

namespace assignment2.Api.Controllers
{
    [ApiController]
    [Route("api/vetdoctors")]
    public class VetDoctorsController : ControllerBase
    {
        private readonly Assignment1DbContext _db;
        public VetDoctorsController(Assignment1DbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VetDoctor>>> GetAll()
            => await _db.VetDoctors.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VetDoctor>> GetOne(int id)
        {
            var doc = await _db.VetDoctors
                .Include(d => d.Pets)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
            return doc is null ? NotFound() : doc;
        }

        [HttpPost]
        public async Task<ActionResult<VetDoctor>> Create(VetDoctor input)
        {
            _db.VetDoctors.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { id = input.Id }, input);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, VetDoctor input)
        {
            if (id != input.Id) return BadRequest();
            _db.Entry(input).State = EntityState.Modified;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.VetDoctors.AnyAsync(d => d.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doc = await _db.VetDoctors.FindAsync(id);
            if (doc is null) return NotFound();
            _db.VetDoctors.Remove(doc);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
