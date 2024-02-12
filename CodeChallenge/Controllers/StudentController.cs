using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly AWDbContext _db;

        public StudentController(AWDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudent student)
        {
            Student newStudent = new Student
            {
                Name = student.Name,
                Age = student.Age,
                HasExperience = student.HasExperience,
                CourseId = student.CourseId,

            };
            _db.Add(newStudent);
           await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudent (int studentId)
        {
            var student = await _db.Student
                .SingleOrDefaultAsync(s=>s.Id == studentId);
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpGet ("{name}")]
        public async Task<IActionResult> GetStudentByName(string name)
        {
            var student = await _db.Student
                .SingleOrDefaultAsync(s => s.Name == name);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpGet("/Students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var listOfStudents = await _db.Student.ToListAsync();
            return Ok(listOfStudents);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = _db.Student.SingleOrDefault(s => s.Id == studentId);
            if(student == null)
            {
                return NotFound();
            }
            _db.Student.Remove(student);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int studentId, CreateStudent student)
        {
            var oldStudent= await _db.Student.SingleOrDefaultAsync(s=> s.Id == studentId);
            if (oldStudent == null)
            {
                return NotFound();
            }

            oldStudent.Name= student.Name;
            oldStudent.CourseId= student.CourseId;
            oldStudent.Age= student.Age;
            oldStudent.HasExperience= student.HasExperience;

            await _db.SaveChangesAsync();
            return Ok(oldStudent);
        }


    }

    public class CreateStudent
    {
        public string Name { get; set; } = string.Empty;

        public bool HasExperience { get; set; }

        public int Age { get; set; }

        public int CourseId { get; set; }
    }
}
