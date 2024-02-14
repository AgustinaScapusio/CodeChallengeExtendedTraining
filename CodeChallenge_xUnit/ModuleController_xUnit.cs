using Xunit;
using CodeChallenge.Controllers;
using CodeChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CodeChallenge_xUnit
{
    public class ModuleControllerTests
    {
        [Fact]
        public async Task GetAllModules_Returns_AllModules()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllModules_Returns_AllModules")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1" });
                context.Module.Add(new Module { Title = "Module 2" });
                context.SaveChanges();

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetAllModules() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var modules = result.Value as List<Module>;
                Assert.Equal(2, modules?.Count);
            }
        }

        [Fact]
        public async Task GetAllModules_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetAllModules_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1" });
                context.SaveChanges();

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetAllModules() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var modules = result.Value as List<Module>;
                Assert.NotEqual(2, modules?.Count);
            }
        }

        [Fact]
        public async Task GetOneModule_Returns_OneModule()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneModule_Returns_OneModule")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1" });
                context.SaveChanges();
                var expectedModule = new Module { Title = "Module 1", ID = 1 };

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetModule(1) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);

                var okResult = result as OkObjectResult;
                var returnedModule = okResult.Value as Module;

                Assert.NotNull(returnedModule);
                Assert.Equal(expectedModule.ID, returnedModule.ID);
                Assert.Equal(expectedModule.Title, returnedModule.Title);
            }
        }

        [Fact]
        public async Task GetOneModuleWithInvalid_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneModuleWithInvalid_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 0 });
                context.SaveChanges();
                var expectedModule = new Module { Title = "Module 1", ID = 1 };

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetModule(1);

                var okResult = result as OkObjectResult;
                var returnedModule = okResult?.Value as Module;

                Assert.NotEqual(expectedModule, returnedModule);
            }
        }

        [Fact]
        public async Task GetOneModuleWithValidName_Returns_Module()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneModuleWithValidName_Returns_Module")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var expectedModule = new Module { Title = "Module 1", ID = 1 };

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetModuleByName("Module 1");

                var okResult = result as OkObjectResult;
                var returnedModule = okResult?.Value as Module;

                Assert.Equal(expectedModule.Title, returnedModule?.Title);
            }
        }

        [Fact]
        public async Task GetOneModuleWithValidName_Returns_Invalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "GetOneModuleWithValidName_Returns_Invalid")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var expectedModule = new Module { Title = "Module 2", ID = 1 };

                var controller = new ModuleController(context);

                // Act
                var result = await controller.GetModuleByName("Module 1");

                var okResult = result as OkObjectResult;
                var returnedModule = okResult?.Value as Module;

                Assert.NotEqual(expectedModule.Title, returnedModule?.Title);
            }
        }

        [Fact]
        public async Task DeleteOneModuleWithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneModuleWithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var controller = new ModuleController(context);

                // Act
                var result = await controller.DeleteModule(1);

                var okResult = result as OkObjectResult;

                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task DeleteOneModuleWithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "DeleteOneModuleWithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var controller = new ModuleController(context);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.DeleteModule(100)); // Assuming 100 is an invalid module ID
            }
        }

        [Fact]
        public async Task UpdateModule_WithValidId_Returns_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateModule_WithValidId_Returns_Success")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var controller = new ModuleController(context);

                // Act
                var result = await controller.UpdateModule(1, new CreateModule { Title = "Updated Title" });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var updatedModule = Assert.IsType<Module>(okResult.Value);
                Assert.Equal("Updated Title", updatedModule.Title);
            }
        }

        [Fact]
        public async Task UpdateModule_WithInvalidId_Returns_Error()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AWDbContext>()
                          .UseInMemoryDatabase(databaseName: "UpdateModule_WithInvalidId_Returns_Error")
                          .Options;

            // Use a clean instance of context for each test
            using (var context = new AWDbContext(options))
            {
                context.Module.Add(new Module { Title = "Module 1", ID = 1 });
                context.SaveChanges();
                var controller = new ModuleController(context);

                // Act
                var result = await controller.UpdateModule(100, new CreateModule { Title = "Updated Title" }); // Assuming 100 is an invalid module ID

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
