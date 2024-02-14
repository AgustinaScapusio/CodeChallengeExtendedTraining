using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge_xUnit
{
    public class CourseControllerTests
    {
        [Fact]
        public async Task GetAllCourses_Returns_AllCourses()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllCourses_Returns_AllCourses")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Course.Add(new Course { Name = "Math", City = "New York", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) });
                context.Course.Add(new Course { Name = "Science", City = "Los Angeles", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) });
                context.SaveChanges();

                var controller = new CourseController(context);

                // Act
                var result = await controller.GetAllCourses() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var courses = result.Value as List<Course>;
                Assert.Equal(2, courses?.Count);
            }
        }

        [Fact]
        public async Task GetCourse_Returns_Course_With_Valid_Id()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetCourse_Returns_Course_With_Valid_Id")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var course = new Course { Name = "Math", City = "New York", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) };
                context.Course.Add(course);
                context.SaveChanges();

                var controller = new CourseController(context);

                // Act
                var result = await controller.GetCourse(course.Id) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var returnedCourse = result.Value as Course;
                Assert.NotNull(returnedCourse);
                Assert.Equal(course.Id, returnedCourse.Id);
            }
        }

        [Fact]
        public async Task GetCourseByName_Returns_Course_With_Valid_Name()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetCourseByName_Returns_Course_With_Valid_Name")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var course = new Course { Name = "Math", City = "New York", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) };
                context.Course.Add(course);
                context.SaveChanges();

                var controller = new CourseController(context);

                // Act
                var result = await controller.GetCourseByName(course.Name) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var returnedCourse = result.Value as Course;
                Assert.NotNull(returnedCourse);
                Assert.Equal(course.Name, returnedCourse.Name);
            }
        }

        [Fact]
        public async Task DeleteCourse_With_Valid_Id_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteCourse_With_Valid_Id_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var course = new Course { Name = "Math", City = "New York", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) };
                context.Course.Add(course);
                context.SaveChanges();

                var controller = new CourseController(context);

                // Act
                var result = await controller.DeleteCourse(course.Id) as NoContentResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(204, result.StatusCode);

                // Check if course is deleted
                var deletedCourse = await context.Course.FindAsync(course.Id);
                Assert.Null(deletedCourse);
            }
        }

        [Fact]
        public async Task UpdateCourse_With_Valid_Id_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateCourse_With_Valid_Id_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                var course = new Course { Name = "Math", City = "New York", FromDate = DateTime.Now, ToDate = DateTime.Now.AddMonths(1) };
                context.Course.Add(course);
                context.SaveChanges();

                var controller = new CourseController(context);

                // Act
                var updatedName = "Mathematics";
                var result = await controller.UpdateCourse(course.Id, new CreateCourse { Name = updatedName }) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var updatedCourse = result.Value as Course;
                Assert.NotNull(updatedCourse);
                Assert.Equal(updatedName, updatedCourse.Name);
            }
        }
    }
}
