using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;

namespace assignment2.Api.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private readonly Assignment1DbContext _db;
        public PetsController(Assignment1DbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetAll()
            => await _db.Pets.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pet>> GetOne(int id)
        {
            var pet = await _db.Pets
                .Include(p => p.VetDoctor)
                .Include(p => p.PetProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return pet is null ? NotFound() : pet;
        }

        [HttpPost]
        public async Task<ActionResult<Pet>> Create(Pet input)
        {
            if (!await _db.VetDoctors.AnyAsync(d => d.Id == input.VetDoctorId))
                return BadRequest("VetDoctorId not found.");

            _db.Pets.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { id = input.Id }, input);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Pet input)
        {
            if (id != input.Id) return BadRequest();
            _db.Entry(input).State = EntityState.Modified;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Pets.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pet = await _db.Pets.FindAsync(id);
            if (pet is null) return NotFound();
            _db.Pets.Remove(pet);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
