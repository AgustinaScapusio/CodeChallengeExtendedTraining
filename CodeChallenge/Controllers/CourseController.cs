using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly AWDbContext _db;

        public CourseController(AWDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourse course)
        {
            Course newCourse = new Course
            {
                Name = course.Name,
                City = course.City,
                FromDate = course.FromDate,
                ToDate = course.ToDate,
            };
            _db.Add(newCourse);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = newCourse.Id }, newCourse);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(int courseId)
        {
            var course= await _db.Course
                .Include(c=>c.CourseModules)!
                .ThenInclude(c=>c.Module)
                .SingleOrDefaultAsync(s => s.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCourseByName(string name)
        {
            var course = await _db.Course
                .Include(c => c.CourseModules)!
                .ThenInclude(c => c.Module)
                .SingleOrDefaultAsync(s => s.Name == name);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpGet("/Courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var listOfCourses = await _db.Course.Include(c => c.CourseModules)!
                .ThenInclude(c => c.Module).ToListAsync();
            return Ok(listOfCourses);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var course = _db.Course.Include(c=>c.CourseModules)!
                .ThenInclude(c=>c.Module)
                .SingleOrDefault(s => s.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }
            _db.Course.Remove(course);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(int courseId, CreateCourse course)
        {
            var oldCourse = await _db.Course.SingleOrDefaultAsync(s => s.Id == courseId);
            if (oldCourse == null)
            {
                return NotFound();
            }

            oldCourse.Name = course.Name;
            oldCourse.City = course.City;
            oldCourse.FromDate = course.FromDate;
            oldCourse.ToDate = course.ToDate;

            await _db.SaveChangesAsync();
            return Ok(oldCourse);
        }

    }
    public class CreateCourse
    {
        public string Name { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
