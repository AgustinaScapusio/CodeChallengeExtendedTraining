using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly AWDbContext _db;

        public TeacherController(AWDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacher(int teacherId)
        {
            var teacher = await _db.Teacher.SingleAsync(m => m.Id == teacherId);

            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetTeacherByName(string name)
        {
            var teacher = await _db.Teacher.SingleAsync(m => m.Name == name);

            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTeacher(CreateTeacher teacher)
        {
            Teacher newTeacher = new Teacher
            {
                Name = teacher.Name,
            };
            _db.Add(newTeacher);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacher), new { id = newTeacher.Id }, newTeacher); ;
        }


        [HttpGet("/Teachers")]
        public async Task<IActionResult> GetAllTeacher()
        {
            var listOfTeacher = await _db.Teacher.ToListAsync();
            return Ok(listOfTeacher);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher(int teacherID)
        {
            var teacher = await _db.Teacher.Include(t=>t.TeacherModules)!
                                            .ThenInclude(t => t.Module)
                                            .SingleAsync(t => t.Id == teacherID);
            if (teacher == null)
            {
                return NotFound();
            }
            _db.Teacher.Remove(teacher);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacher(int teacherId, CreateTeacher teacher)
        {
            var oldTeacher = await _db.Teacher.SingleOrDefaultAsync(m => m.Id == teacherId);
            if (oldTeacher == null)
            {
                return NotFound();
            }
            oldTeacher.Name = teacher.Name;

            await _db.SaveChangesAsync();
            return Ok(oldTeacher);
        }

    }
    public class CreateTeacher
    {
        public string Name { get; set; } = string.Empty;

    }
}
