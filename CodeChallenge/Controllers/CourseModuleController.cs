using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{
        [Route("/[controller]")]
        [ApiController]
    public class CourseModuleController : Controller
       
    {
            private readonly AWDbContext _db;

            public CourseModuleController(AWDbContext db)
            {
                _db = db;
            }

            [HttpPost]
            public async Task<IActionResult> CreateCourseModule(CreateCourseModule courseModule)
            {
                CourseModule newCourseModule = new CourseModule
                {
                    ModuleID = courseModule.ModuleID,
                    CourseID = courseModule.CourseID,
                };
                _db.Add(newCourseModule);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCourseModule), new { id = newCourseModule.ID }, newCourseModule);
            }

            [HttpGet]
            public async Task<IActionResult> GetCourseModule(int courseModuleId)
            {
                var courseModule= await _db.CourseModule.SingleAsync(c=>c.ID==courseModuleId);
                if (courseModule == null)
                {
                    return NotFound();
                }
                return Ok(courseModule);
            }

            [HttpGet("/CourseModules")]
            public async Task<IActionResult> GetAllCourseModules()
            {
                var listOfCourseModules = await _db.CourseModule.ToListAsync();
                return Ok(listOfCourseModules);
            }
            [HttpDelete]
            public async Task<IActionResult> DeleteCourseModule(int courseModulesID)
            {
                var courseModules = _db.CourseModule
                    .SingleOrDefault(c => c.ID == courseModulesID);
                if (courseModules == null)
                {
                    return NotFound();
                }
                _db.CourseModule.Remove(courseModules);
                await _db.SaveChangesAsync();
                return NoContent();
            }

            [HttpPut]
            public async Task<IActionResult> UpdateCourseModule(int courseModuleId, CreateCourseModule courseModule)
            {
                var oldCourseModule = await _db.CourseModule.SingleOrDefaultAsync(c => c.ID == courseModuleId);
                if (oldCourseModule == null)
                {
                    return NotFound();
                }
                    oldCourseModule.ModuleID = courseModule.ModuleID;
                    oldCourseModule.CourseID = courseModule.CourseID;

                await _db.SaveChangesAsync();
                return Ok(oldCourseModule);
            }

        }
        public class CreateCourseModule
        {
             public int ModuleID { get; set; }

             public int CourseID { get; set; }

        }
}
