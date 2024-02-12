using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeChallenge.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ModuleController : Controller
    {
        private readonly AWDbContext _db;

        public ModuleController(AWDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetModule(int moduleId)
        {
            var module = await _db.Module.SingleAsync(m=>m.ID == moduleId);
                                
            if(module==null)
            {
                return NotFound();
            }
            return Ok(module);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule (CreateModule module)
        {
            Module newModule = new Module
            {
                Title = module.Title,
            };
            _db.Add(newModule);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetModule), new { id = newModule.ID }, newModule);
        }

        [HttpGet("/Modules")]
        public async Task<IActionResult> GetAllModules()
        {
            var listOfModules = await _db.Module.ToListAsync();
            return Ok(listOfModules);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModule(int moduleID)
        {
            var module = await _db.Module.Include(m => m.CourseModule)!
                                            .ThenInclude(m => m.Module)
                                            .Include(m => m.TeacherModules)!
                                            .ThenInclude(m => m.Teacher)
                                            .SingleAsync(m => m.ID == moduleID);
            if (module == null)
            {
                return NotFound();
            }
            _db.Module.Remove(module);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateModule(int moduleId, CreateModule module)
        {
            var oldModule = await _db.Module.SingleOrDefaultAsync(m => m.ID == moduleId);
            if (oldModule == null)
            {
                return NotFound();
            }
            oldModule.Title = module.Title;

            await _db.SaveChangesAsync();
            return Ok(oldModule);
        }
    }

    public class CreateModule
    {
        public string Title { get; set; } = string.Empty;

    }
}
