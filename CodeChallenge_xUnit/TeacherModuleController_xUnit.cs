using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge_xUnit
{
    public class TeacherModuleControllerTests
    {
        [Fact]
        public async Task GetAllTeacherModules_Returns_AllTeacherModules()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllTeacherModules_Returns_AllTeacherModules")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.TeacherModules.Add(new TeacherModule { ModuleID = 1, TeacherID = 1 });
                context.TeacherModules.Add(new TeacherModule { ModuleID = 2, TeacherID = 2 });
                context.SaveChanges();

                var controller = new TeacherModuleController(context);

                // Act
                var result = await controller.GetAllTeacherModules() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var teacherModules = result.Value as List<TeacherModule>;
                Assert.Equal(2, teacherModules?.Count);
            }
        }

        [Fact]
        public async Task GetTeacherModule_Returns_TeacherModule_With_Valid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetTeacherModule_Returns_TeacherModule_With_Valid_Id")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var teacherModule = new TeacherModule { ModuleID = 1, TeacherID = 1 };
                context.TeacherModules.Add(teacherModule);
                context.SaveChanges();

                var controller = new TeacherModuleController(context);

                // Act
                var result = await controller.GetTeacherModule(teacherModule.ID) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var returnedTeacherModule = result.Value as TeacherModule;
                Assert.NotNull(returnedTeacherModule);
                Assert.Equal(teacherModule.ID, returnedTeacherModule.ID);
            }
        }

        [Fact]
        public async Task DeleteTeacherModule_With_Valid_Id_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteTeacherModule_With_Valid_Id_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var controller = new TeacherModuleController(context);
                var teacherModule = new TeacherModule { ModuleID = 1, TeacherID = 1 };
                context.TeacherModules.Add(teacherModule);
                context.SaveChanges();


                // Act
                var result = await controller.DeleteTeacherModule(1) as NoContentResult;


                // Assert
                Assert.NotNull(result);
                Assert.Equal(204, result.StatusCode);

                // Check if teacher module is deleted
                var deletedTeacherModule = await context.TeacherModules.FindAsync(teacherModule.ID);
                Assert.Null(deletedTeacherModule);
            }
        }

        [Fact]
        public async Task UpdateTeacherModule_With_Valid_Id_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateTeacherModule_With_Valid_Id_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var teacherModule = new TeacherModule { ModuleID = 1, TeacherID = 1 };
                context.TeacherModules.Add(teacherModule);
                context.SaveChanges();

                var controller = new TeacherModuleController(context);

                // Act
                var updatedModuleId = 2;
                var result = await controller.UpdateModule(teacherModule.ID, new CreateTeacherModule { ModuleID = updatedModuleId, TeacherID = teacherModule.TeacherID }) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var updatedTeacherModule = result.Value as TeacherModule;
                Assert.NotNull(updatedTeacherModule);
                Assert.Equal(updatedModuleId, updatedTeacherModule.ModuleID);
            }
        }
        
        [Fact]
        public async Task UpdateTeacherModule_With_Invalid_Id_Returns_NotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateTeacherModule_With_Invalid_Id_Returns_NotFound")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var controller = new TeacherModuleController(context);

                // Act
                var result = await controller.UpdateModule(999, new CreateTeacherModule { ModuleID = 1, TeacherID = 1 }) as NotFoundResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(404, result.StatusCode);
            }
        }
    }
}
    
