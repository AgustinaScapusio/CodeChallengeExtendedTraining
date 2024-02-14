using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CodeChallenge_xUnit
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task GetAllStudents_Returns_AllStudents()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllStudents_Returns_AllStudents")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John" });
                context.Student.Add(new Student { Name = "Jane" });
                context.SaveChanges();

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetAllStudents() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var students = result.Value as List<Student>;
                Assert.Equal(2, students?.Count);
            }
        }

        [Fact]
        public async Task GetAllStudents_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllStudents_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John" });
                context.SaveChanges();

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetAllStudents() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var students = result.Value as List<Student>;
                Assert.NotEqual(2, students?.Count);
            }
        }

        [Fact]
        public async Task GetOneStudent_Returns_OneStudent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneStudent_Returns_OneStudent")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John" });
                context.SaveChanges();
                var expectedStudent = new Student { Name = "John", Id = 1 };

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetStudent(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);

                var okResult = result as OkObjectResult;
                var returnedStudent = okResult.Value as Student;

                Assert.NotNull(returnedStudent);
                Assert.Equal(expectedStudent.Id, returnedStudent.Id);
                Assert.Equal(expectedStudent.Name, returnedStudent.Name);
            }
        }

        [Fact]
        public async Task GetOneStudentWithInvalid_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneStudentWithInvalid_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 0 });
                context.SaveChanges();
                var expectedStudent = new Student { Name = "John", Id = 1 };

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetStudent(1);

                var okResult = result as OkObjectResult;
                var returnedStudent = okResult?.Value as Student;

                Assert.NotEqual(expectedStudent, returnedStudent);
            }
        }

        [Fact]
        public async Task GetOneStudentWithValidName_Returns_Student()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneStudentWithValidName_Returns_Student")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var expectedStudent = new Student { Name = "John", Id = 1 };

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetStudentByName("John");

                var okResult = result as OkObjectResult;
                var returnedStudent = okResult?.Value as Student;

                Assert.Equal(expectedStudent.Name, returnedStudent?.Name);
            }   
        }

        [Fact]
        public async Task GetOneStudentWithValidName_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneStudentWithValidName_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var expectedStudent = new Student { Name = "Jane", Id = 1 };

                var controller = new StudentController(context);

                // Act
                var result = await controller.GetStudentByName("John");

                var okResult = result as OkObjectResult;
                var returnedStudent = okResult?.Value as Student;

                Assert.NotEqual(expectedStudent.Name, returnedStudent?.Name);
            }
        }

        [Fact]
        public async Task DeleteOneStudentWithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneStudentWithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var controller = new StudentController(context);

                // Act
                var result = await controller.DeleteStudent(1);

                var okResult = result as OkObjectResult;

                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task DeleteOneStudentWithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneStudentWithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var controller = new StudentController(context);

                // Act & Assert
                var result = await controller.DeleteStudent(100);
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task UpdateStudent_WithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateStudent_WithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var controller = new StudentController(context);

                // Act
                var result = await controller.UpdateStudent(1, new CreateStudent { Name = "Updated Name" });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var updatedStudent = Assert.IsType<Student>(okResult.Value);
                Assert.Equal("Updated Name", updatedStudent.Name);
            }
        }

        [Fact]
        public async Task UpdateStudent_WithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateStudent_WithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Student.Add(new Student { Name = "John", Id = 1 });
                context.SaveChanges();
                var controller = new StudentController(context);

                // Act
                var result = await controller.UpdateStudent(100, new CreateStudent { Name = "Updated Name" }); // Assuming 100 is an invalid student ID

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}