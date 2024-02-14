using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers
{

    [Route("/[controller]")]
    [ApiController]
    public class TeacherModuleController : Controller
    {
        private readonly AWDbContext _db;

        public TeacherModuleController(AWDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> GetTeacherModule(int teacherModuleId)
        {
            var teacherModule = await _db.TeacherModules.SingleAsync(m => m.ID == teacherModuleId);

            if (teacherModule == null)
            {
                return NotFound();
            }
            return Ok(teacherModule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacherModule(CreateTeacherModule teacherModule)
        {
            TeacherModule newTeacherModule = new TeacherModule
            {
              ModuleID = teacherModule.ModuleID,
              TeacherID= teacherModule.TeacherID,

            };
            _db.Add(newTeacherModule);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacherModule), new { id = newTeacherModule.ID }, newTeacherModule);
        }

        [HttpGet("/TeacherModules")]
        public async Task<IActionResult> GetAllTeacherModules()
        {
            var listOfTeacherModules = await _db.TeacherModules.ToListAsync();
            return Ok(listOfTeacherModules);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacherModule(int teacherModuleID)
        {
            var teacherModule = await _db.TeacherModules.SingleAsync(m => m.ID == teacherModuleID);
            if (teacherModule == null)
            {
                return NotFound();
            }
            _db.Remove(teacherModule);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateModule(int teacherModuleId, CreateTeacherModule teacherModule)
        {
            var oldTeacherModule = await _db.TeacherModules.SingleOrDefaultAsync(m => m.ID == teacherModuleId);
            if (oldTeacherModule == null)
            {
                return NotFound();
            }
            oldTeacherModule.ModuleID = teacherModule.ModuleID;
            oldTeacherModule.TeacherID= teacherModule.TeacherID;

            await _db.SaveChangesAsync();
            return Ok(oldTeacherModule);
        }
    }

    public class CreateTeacherModule
    {
        public int ModuleID { get; set; }

        public int TeacherID { get; set; }


    }
}