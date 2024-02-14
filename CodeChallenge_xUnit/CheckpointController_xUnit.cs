using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using CodeChallenge.Models;

namespace CodeChallenge_xUnit
{
    public class CheckpointControllerTests
    {
        [Fact]
        public async Task GetAllCheckpoints_Returns_AllCheckpoints()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllCheckpoints_Returns_AllCheckpoints")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1 });
                context.Checkpoint.Add(new Checkpoint { Score = 6, ModuleId = 2 });
                context.SaveChanges();

                var controller = new CheckpointController(context);

                // Act
                var result = await controller.GetAllCheckpoint() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var checkpoints = result.Value as List<Checkpoint>;
                Assert.Equal(2, checkpoints?.Count);
            }
        }

        [Fact]
        public async Task GetAllCheckpoints_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllCheckpoints_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1 });
                context.SaveChanges();

                var controller = new CheckpointController(context);

                // Act
                var result = await controller.GetAllCheckpoint() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var checkpoints = result.Value as List<Checkpoint>;
                Assert.NotEqual(2, checkpoints?.Count);
            }
        }

        [Fact]
        public async Task GetOneCheckpoint_Returns_OneCheckpoint()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneCheckpoint_Returns_OneCheckpoint")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1 });
                context.SaveChanges();
                var expectedCheckpoint = new Checkpoint { Score = 8, ModuleId = 1, Id = 1 };

                var controller = new CheckpointController(context);

                // Act
                var result = await controller.GetCheckpoint(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);

                var okResult = result as OkObjectResult;
                var returnedCheckpoint = okResult.Value as Checkpoint;

                Assert.NotNull(returnedCheckpoint);
                Assert.Equal(expectedCheckpoint.Id, returnedCheckpoint.Id);
                Assert.Equal(expectedCheckpoint.Score, returnedCheckpoint.Score);
                Assert.Equal(expectedCheckpoint.ModuleId, returnedCheckpoint.ModuleId);
            }
        }

        [Fact]
        public async Task GetOneCheckpointWithInvalid_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneCheckpointWithInvalid_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 0 });
                context.SaveChanges();
                var expectedCheckpoint = new Checkpoint { Score = 8, ModuleId = 1, Id = 1 };

                var controller = new CheckpointController(context);

                // Act
                var result = await controller.GetCheckpoint(1);

                var okResult = result as OkObjectResult;
                var returnedCheckpoint = okResult?.Value as Checkpoint;

                Assert.NotEqual(expectedCheckpoint, returnedCheckpoint);
            }
        }

        [Fact]
        public async Task DeleteOneCheckpointWithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneCheckpointWithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 1 });
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 2 });

                await context.SaveChangesAsync();

                var controller = new CheckpointController(context);

                // Act
                var result = await controller.DeleteCheckpoint(1);

                Assert.IsType<NoContentResult>(result);
            }
        }


        [Fact]
        public async Task DeleteOneCheckpointWithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneCheckpointWithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 1 });
                context.SaveChanges();
                var controller = new CheckpointController(context);

                // Act & Assert
                var result = await controller.DeleteCheckpoint(100);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task UpdateCheckpoint_WithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateCheckpoint_WithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 1 });
                context.SaveChanges();
                var controller = new CheckpointController(context);

                // Act
                var result = await controller.UpdateCheckpoint(1, new CreateCheckpoint { Score = 10, ModuleId = 2 });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var updatedCheckpoint = Assert.IsType<Checkpoint>(okResult.Value);
                Assert.Equal(10, updatedCheckpoint.Score);
                Assert.Equal(2, updatedCheckpoint.ModuleId);
            }
        }

        [Fact]
        public async Task UpdateCheckpoint_WithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateCheckpoint_WithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Checkpoint.Add(new Checkpoint { Score = 8, ModuleId = 1, Id = 1 });
                context.SaveChanges();
                var controller = new CheckpointController(context);

                // Act
                var result = await controller.UpdateCheckpoint(100, new CreateCheckpoint { Score = 10, ModuleId = 2 }); // Assuming 100 is an invalid checkpoint ID

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
