using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using CodeChallenge.Models;

namespace CodeChallenge_xUnit
{
    public class CheckpointStudentControllerTests
    {
        [Fact]
        public async Task GetAllCheckpointStudents_Returns_AllCheckpointStudents()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllCheckpointStudents_Returns_AllCheckpointStudents")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CheckpointStudent.Add(new CheckpointStudent { StudentID = 1, CheckpointID = 1 });
                context.CheckpointStudent.Add(new CheckpointStudent { StudentID = 2, CheckpointID = 2 });
                context.SaveChanges();

                var controller = new CheckpointStudentController(context);

                // Act
                var result = await controller.GetAllCheckpointStudents() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var checkpointStudents = result.Value as List<CheckpointStudent>;
                Assert.Equal(2, checkpointStudents?.Count);
            }
        }

        [Fact]
        public async Task CreateCheckpointStudent_Returns_CreatedAtActionResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "CreateCheckpointStudent_Returns_CreatedAtActionResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var controller = new CheckpointStudentController(context);
                var checkpointStudent = new CreateCheckpointStudent { StudentID = 1, CheckpointID = 1 };

                // Act
                var result = await controller.CreateCheckpointStudent(checkpointStudent) as CreatedAtActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("GetCheckpointStudent", result.ActionName);
                Assert.NotNull(result.RouteValues);
                Assert.Equal(1, result.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task GetCheckpointStudent_Returns_OneCheckpointStudent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetCheckpointStudent_Returns_OneCheckpointStudent")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CheckpointStudent.Add(new CheckpointStudent { Id = 1, StudentID = 1, CheckpointID = 1 });
                context.SaveChanges();

                var controller = new CheckpointStudentController(context);

                // Act
                var result = await controller.GetCheckpointStudent(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var checkpointStudent = result.Value as CheckpointStudent;
                Assert.NotNull(checkpointStudent);
                Assert.Equal(1, checkpointStudent.Id);
            }
        }

        [Fact]
        public async Task DeleteCheckpointStudent_Returns_OkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteCheckpointStudent_Returns_OkResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CheckpointStudent.Add(new CheckpointStudent {  StudentID = 1, CheckpointID = 1 });
                context.CheckpointStudent.Add(new CheckpointStudent {  StudentID = 2, CheckpointID = 1 });
                await context.SaveChangesAsync();

                var controller = new CheckpointStudentController(context);

                // Act
                var result = await controller.DeleteCheckpointStudent(1);
                // Assert
              
                Assert.IsType<OkObjectResult>(result);
            }
        }


        [Fact]
        public async Task UpdateCheckpointStudent_Returns_OkObjectResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateCheckpointStudent_Returns_OkObjectResult")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                // Add sample data
                context.CheckpointStudent.Add(new CheckpointStudent { Id = 1, StudentID = 1, CheckpointID = 1 });
                context.SaveChanges();

                var controller = new CheckpointStudentController(context);
                var checkpointStudent = new CreateCheckpointStudent { StudentID = 2, CheckpointID = 2 };

                // Act
                var result = await controller.UpdateCheckpointStudent(1, checkpointStudent) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var updatedCheckpointStudent = result.Value as CheckpointStudent;
                Assert.NotNull(updatedCheckpointStudent);
                Assert.Equal(2, updatedCheckpointStudent.StudentID);
                Assert.Equal(2, updatedCheckpointStudent.CheckpointID);
            }
        }
    }
}
