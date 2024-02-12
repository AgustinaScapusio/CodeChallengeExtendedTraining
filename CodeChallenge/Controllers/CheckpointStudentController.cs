using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CheckpointStudentController : Controller
    {
        private readonly AWDbContext _db;

        public CheckpointStudentController(AWDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckpointStudent(CreateCheckpointStudent createCheckpointStudent)
        {
            CheckpointStudent checkpointStudent = new CheckpointStudent
            {
                StudentID = createCheckpointStudent.StudentID,
                CheckpointID = createCheckpointStudent.CheckpointID,
            }; 
            _db.Add(checkpointStudent);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCheckpointStudent), new { id = checkpointStudent.Id }, checkpointStudent);
        }

        [HttpGet("{checkpointStudentId}")]
        public async Task<IActionResult> GetCheckpointStudent(int checkpointStudentId)
        {
            var checkpointStudent = await _db.CheckpointStudent.FirstOrDefaultAsync(c=>c.Id == checkpointStudentId);

            if (checkpointStudent == null)
            {
                return NotFound();
            }
            return Ok(checkpointStudent);
        }

        [HttpGet("/CheckpointsStudents")]
        public async Task<IActionResult> GetAllCheckpointStudents()
        {
            var listOfCheckpointStudents = await _db.CheckpointStudent.ToListAsync();
            return Ok(listOfCheckpointStudents);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCheckpointStudent(int checkpointStudentId)
        {
            var checkpointStudent = _db.CheckpointStudent.Include(c => c.Student).Include(c => c.Checkpoint).FirstOrDefaultAsync(c => c.Id == checkpointStudentId);
            if (checkpointStudent == null)
            {
                return NotFound();
            }
            _db.Remove(checkpointStudent);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCheckpointStudent(int checkpointStudentId, CreateCheckpointStudent checkpointStudent)
        {
            var oldCheckpointStudent = await _db.CheckpointStudent.SingleOrDefaultAsync(s => s.Id == checkpointStudentId);
            if (oldCheckpointStudent == null)
            {
                return NotFound();
            }
            oldCheckpointStudent.StudentID = checkpointStudent.StudentID;
            oldCheckpointStudent.CheckpointID = checkpointStudent.CheckpointID;

            await _db.SaveChangesAsync();
            return Ok(oldCheckpointStudent);
        }

    }
    public class CreateCheckpointStudent
    {
        public int StudentID { get; set; }

        public int CheckpointID { get; set; }
    }
}
