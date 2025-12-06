using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;

namespace assignment2.Api.Controllers
{
    [ApiController]
    [Route("api/petprofiles")]
    public class PetProfilesController : ControllerBase
    {
        private readonly Assignment1DbContext _db;
        public PetProfilesController(Assignment1DbContext db) => _db = db;

        // GET /api/petprofiles/{petId}
        [HttpGet("{petId:int}")]
        public async Task<ActionResult<PetProfile>> GetByPet(int petId)
        {
            var profile = await _db.PetProfiles
                .Include(pp => pp.Pet)
                .AsNoTracking()
                .FirstOrDefaultAsync(pp => pp.PetId == petId);

            return profile is null ? NotFound() : profile;
        }

        // POST /api/petprofiles  (create or overwrite)
        [HttpPost]
        public async Task<ActionResult<PetProfile>> CreateOrOverwrite(PetProfile input)
        {
            if (!await _db.Pets.AnyAsync(p => p.Id == input.PetId))
                return BadRequest("PetId not found.");

            var existing = await _db.PetProfiles.FirstOrDefaultAsync(pp => pp.PetId == input.PetId);
            if (existing is null)
            {
                _db.PetProfiles.Add(input);
            }
            else
            {
                existing.VetNotes = input.VetNotes;
                _db.Entry(existing).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByPet), new { petId = input.PetId }, input);
        }

        // PUT /api/petprofiles/{petId} (update notes only)
        [HttpPut("{petId:int}")]
        public async Task<IActionResult> UpdateNotes(int petId, [FromBody] string notes)
        {
            var existing = await _db.PetProfiles.FirstOrDefaultAsync(pp => pp.PetId == petId);
            if (existing is null) return NotFound();

            existing.VetNotes = notes ?? string.Empty;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/petprofiles/{petId}
        [HttpDelete("{petId:int}")]
        public async Task<IActionResult> DeleteByPet(int petId)
        {
            var existing = await _db.PetProfiles.FirstOrDefaultAsync(pp => pp.PetId == petId);
            if (existing is null) return NotFound();

            _db.PetProfiles.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
