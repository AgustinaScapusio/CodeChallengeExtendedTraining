using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge_xUnit
{
    public class CourseModuleControllerTests
    {
        [Fact]
        public async Task GetAllCourseModules_Returns_AllCourseModules()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllCourseModules_Returns_AllCourseModules")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CourseModule.Add(new CourseModule { ID = 1, ModuleID = 1, CourseID = 1 });
                context.CourseModule.Add(new CourseModule { ID = 2, ModuleID = 2, CourseID = 2 });
                context.SaveChanges();

                var controller = new CourseModuleController(context);

                // Act
                var result = await controller.GetAllCourseModules() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var courseModules = result.Value as List<CourseModule>;
                Assert.Equal(2, courseModules?.Count);
            }
        }

        [Fact]
        public async Task CreateCourseModule_Returns_CreatedAtActionResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "CreateCourseModule_Returns_CreatedAtActionResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var controller = new CourseModuleController(context);
                var courseModule = new CreateCourseModule { ModuleID = 1, CourseID = 1 };

                // Act
                var result = await controller.CreateCourseModule(courseModule) as CreatedAtActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("GetCourseModule", result.ActionName);
                Assert.NotNull(result.RouteValues);
                Assert.Equal(1, result.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task GetCourseModule_Returns_OneCourseModule()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetCourseModule_Returns_OneCourseModule")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CourseModule.Add(new CourseModule { ID = 1, ModuleID = 1, CourseID = 1 });
                context.SaveChanges();

                var controller = new CourseModuleController(context);

                // Act
                var result = await controller.GetCourseModule(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var courseModule = result.Value as CourseModule;
                Assert.NotNull(courseModule);
                Assert.Equal(1, courseModule.ID);
            }
        }

        [Fact]
        public async Task DeleteCourseModule_Returns_NoContentResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteCourseModule_Returns_NoContentResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CourseModule.Add(new CourseModule { ID=2, ModuleID = 1, CourseID = 1 });
                context.SaveChanges();

                var controller = new CourseModuleController(context);

                // Act
                var result = await controller.DeleteCourseModule(2);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task UpdateCourseModule_Returns_OkObjectResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateCourseModule_Returns_OkObjectResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CourseModule.Add(new CourseModule { ModuleID = 1, CourseID = 1 });
                context.SaveChanges();

                var controller = new CourseModuleController(context);
                var courseModule = new CreateCourseModule { ModuleID = 2, CourseID = 2 };

                // Act
                var result = await controller.UpdateCourseModule(1, courseModule) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var updatedCourseModule = result.Value as CourseModule;
                Assert.NotNull(updatedCourseModule);
                Assert.Equal(2, updatedCourseModule.ModuleID);
                Assert.Equal(2, updatedCourseModule.CourseID);
            }
        }
    }
}
