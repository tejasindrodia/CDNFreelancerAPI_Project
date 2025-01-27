using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDNFreelancerAPI;
using CDNFreelancerAPI.Models;

namespace CDNFreelancerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreelancersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FreelancersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFreelancers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var freelancers = await _context.Freelancers
                .Include(f => f.Skillsets)
                .Include(f => f.Hobbies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(freelancers);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFreelancer([FromBody] Freelancer freelancer)
        {
            await _context.Freelancers.AddAsync(freelancer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFreelancers), new { id = freelancer.Id }, freelancer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFreelancer(int id, [FromBody] Freelancer updatedFreelancer)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null) return NotFound();

            freelancer.Username = updatedFreelancer.Username;
            freelancer.Email = updatedFreelancer.Email;
            freelancer.PhoneNumber = updatedFreelancer.PhoneNumber;

            _context.Skillsets.RemoveRange(_context.Skillsets.Where(s => s.FreelancerId == id));
            _context.Hobbies.RemoveRange(_context.Hobbies.Where(h => h.FreelancerId == id));

            await _context.Skillsets.AddRangeAsync(updatedFreelancer.Skillsets);
            await _context.Hobbies.AddRangeAsync(updatedFreelancer.Hobbies);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreelancer(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null) return NotFound();

            _context.Freelancers.Remove(freelancer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFreelancers([FromQuery] string query)
        {
            var freelancers = await _context.Freelancers
                .Where(f => f.Username.Contains(query) || f.Email.Contains(query))
                .ToListAsync();
            return Ok(freelancers);
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> ArchiveFreelancer(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null) return NotFound();

            freelancer.IsArchived = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/unarchive")]
        public async Task<IActionResult> UnarchiveFreelancer(int id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null) return NotFound();

            freelancer.IsArchived = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}