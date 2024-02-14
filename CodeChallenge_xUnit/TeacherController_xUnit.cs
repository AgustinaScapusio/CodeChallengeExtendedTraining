using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FakeItEasy;
using System.Linq.Expressions;
using Castle.Core.Resource;
using System.Data;

namespace CodeChallenge_xUnit
{
    public class TeacherController_xUnit
    {
        [Fact]
        public async Task GetAllTeacher_Returns_AllTeachers()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllTeacher_Returns_AllTeachers")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas" });
                context.Teacher.Add(new Teacher { Name = "Anders" });
                context.SaveChanges();

                var controller = new TeacherController(context);

                // Act
                var result = await controller.GetAllTeacher() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var teachers = result.Value as List<Teacher>;
                Assert.Equal(2, teachers?.Count);
            }
        }

        [Fact]
        public async Task GetAllTeacher_Returns_Invalid()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllTeacher_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas" });
                context.SaveChanges();

                var controller = new TeacherController(context);

                // Act
                var result = await controller.GetAllTeacher() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var teachers = result.Value as List<Teacher>;
                Assert.NotEqual(2, teachers?.Count);
            }
        }
            [Fact]
            public async Task GetOneTeacher_Returns_OneTeacher()
            {  // Arrange
                var options = new DbContextOptionsBuilder<AWDbContext>()
                              .UseInMemoryDatabase(databaseName: "GetOneTeacher_Returns_OneTeacher")
                              .Options;

                // Use a clean instance of context for each test
                using (var context = new AWDbContext(options))
                {
                    context.Teacher.Add(new Teacher {Name = "Tomas"});
                    context.SaveChanges();
                    var expectedTeacher= new Teacher {Name="Tomas",Id = 1 };

                    var controller = new TeacherController(context);

                    // Act
                    var result = await controller.GetTeacher(1) as OkObjectResult;

                    // Assert
                    Assert.NotNull(result);
                    Assert.IsType<OkObjectResult>(result);

                    var okResult = result as OkObjectResult;
                    var returnedTeacher = okResult.Value as Teacher;

                    Assert.NotNull(returnedTeacher);
                    Assert.Equal(expectedTeacher.Id, returnedTeacher.Id);
                    Assert.Equal(expectedTeacher.Name, returnedTeacher.Name);
                }
            }
        [Fact]
        public async Task GetOneTeacherWithInvalid_Returns_Invalid()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneTeacherWithInvalid_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas",Id=0 });
                context.SaveChanges();
                var expectedTeacher = new Teacher { Name = "Tomas", Id = 1 };

                var controller = new TeacherController(context);

                // Act
                var result = await controller.GetTeacher(1);

                var okResult = result as OkObjectResult;
                var returnedTeacher = okResult?.Value as Teacher;

                Assert.NotEqual(expectedTeacher, returnedTeacher);
            }
        }
        [Fact]
        public async Task GetOneTeacherWithValidName_Returns_Teacher()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneTeacherWithValidName_Returns_Teacher")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas", Id = 1 });
                context.SaveChanges();
                var expectedTeacher = new Teacher { Name = "Tomas", Id = 1 };

                var controller = new TeacherController(context);

                // Act
                var result = await controller.GetTeacherByName("Tomas");

                var okResult = result as OkObjectResult;
                var returnedTeacher = okResult?.Value as Teacher;

                Assert.Equal(expectedTeacher.Name, returnedTeacher?.Name);
            }
        }
        [Fact]
        public async Task GetOneTeacherWithValidName_Returns_Invalid()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneTeacherWithValidName_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas", Id = 1 });
                context.SaveChanges();
                var expectedTeacher = new Teacher { Name = "Anders", Id = 1 };

                var controller = new TeacherController(context);

                // Act
                var result = await controller.GetTeacherByName("Tomas");

                var okResult = result as OkObjectResult;
                var returnedTeacher = okResult?.Value as Teacher;

                Assert.NotEqual(expectedTeacher.Name, returnedTeacher?.Name);
            }
        }
        [Fact]
        public async Task DeleteOneTeacherWithValidId_Returns_Success()
        {  // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneTeacherWithValidName_Returns_Teacher")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Anders", Id = 2 });
                context.SaveChanges();
                var controller = new TeacherController(context);

                // Act
                var result = await controller.DeleteTeacher(2);

                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task DeleteOneTeacherWithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneTeacherWithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas", Id = 1 });
                context.SaveChanges();
                var controller = new TeacherController(context);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.DeleteTeacher(100)); // Assuming 100 is an invalid teacher ID
            }
        }
        [Fact]
        public async Task UpdateTeacher_WithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateTeacher_WithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas", Id = 1 });
                context.SaveChanges();
                var controller = new TeacherController(context);

                // Act
                var result = await controller.UpdateTeacher(1, new CreateTeacher { Name = "Updated Name" });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var updatedTeacher = Assert.IsType<Teacher>(okResult.Value);
                Assert.Equal("Updated Name", updatedTeacher.Name);
            }
        }

        [Fact]
        public async Task UpdateTeacher_WithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateTeacher_WithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Teacher.Add(new Teacher { Name = "Tomas", Id = 1 });
                context.SaveChanges();
                var controller = new TeacherController(context);

                // Act
                var result = await controller.UpdateTeacher(100, new CreateTeacher { Name = "Updated Name" }); // Assuming 100 is an invalid teacher ID

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
