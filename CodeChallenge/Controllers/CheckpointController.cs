using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CheckpointController : Controller
    {
        private readonly AWDbContext _db;

        public CheckpointController(AWDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckpoint(CreateCheckpoint createCheckpoint)
        {
            Checkpoint newCheckpoint = new Checkpoint
            {
                Score = createCheckpoint.Score,
                ModuleId = createCheckpoint.ModuleId,
            };
            _db.Add(newCheckpoint);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCheckpoint), new { id = newCheckpoint.Id }, newCheckpoint);
        }

        [HttpGet("{checkpointId}")]
        public async Task<IActionResult> GetCheckpoint(int checkpointId)
        {
            var checkpoint = await _db.Checkpoint.FirstOrDefaultAsync(c=> c.Id == checkpointId);
               
            if (checkpoint == null)
            {
                return NotFound();
            }
            return Ok(checkpoint);
        }

        [HttpGet("/Checkpoints")]
        public async Task<IActionResult> GetAllCheckpoint()
        {
            var listOfCheckpoints = await _db.Checkpoint.ToListAsync();
            return Ok(listOfCheckpoints);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCheckpoint(int checkpointId)
        {
            var checkpoint = _db.Checkpoint.Include(c=>c.Module).Include(c=>c.CheckpointStudents).FirstOrDefaultAsync(c=>c.Id == checkpointId);
            if (checkpoint == null)
            {
                return NotFound();
            }
            _db.Remove(checkpoint);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCheckpoint(int checkpointId, CreateCheckpoint checkpoint)
        {
            var oldCheckpoint = await _db.Checkpoint.SingleOrDefaultAsync(s => s.Id == checkpointId);
            if (oldCheckpoint == null)
            {
                return NotFound();
            }
            oldCheckpoint.ModuleId = checkpoint.ModuleId;
            oldCheckpoint.Score = checkpoint.Score;

            await _db.SaveChangesAsync();
            return Ok(oldCheckpoint);
        }

    }
}
    public class CreateCheckpoint
    {
        [Range(0, 10)]
        public int Score { get; set; }

        public int ModuleId;
     
    }

