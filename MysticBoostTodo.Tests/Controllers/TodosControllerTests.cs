// Controllers/TodosControllerTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MysticBoostTodo.Api.Controllers;
using MysticBoostTodo.Core.Entities;
using MysticBoostTodo.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MysticBoostTodo.Tests.Controllers
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _mockService;
        private readonly TodosController _controller;

        public TodosControllerTests()
        {
            _mockService = new Mock<ITodoService>();
            _controller = new TodosController(_mockService.Object);
        }

        [Fact]
        public async Task GetTodos_ShouldReturnOkResult()
        {
            // Arrange
            var todos = new List<Todo>
            {
                new Todo { Id = 1, Title = "Task 1" },
                new Todo { Id = 2, Title = "Task 2" }
            };

            _mockService.Setup(service => service.GetAllTodosAsync())
                .ReturnsAsync(todos);

            // Act
            var result = await _controller.GetTodos();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(todos);
        }

        [Fact]
        public async Task GetTodo_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Task 1" };
            _mockService.Setup(service => service.GetTodoByIdAsync(1))
                .ReturnsAsync(todo);

            // Act
            var result = await _controller.GetTodo(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(todo);
        }

        [Fact]
        public async Task GetTodo_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetTodoByIdAsync(999))
                .ReturnsAsync((Todo)null);

            // Act
            var result = await _controller.GetTodo(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateTodo_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var todo = new Todo { Title = "New Task" };
            var createdTodo = new Todo { Id = 1, Title = "New Task" };

            _mockService.Setup(service => service.CreateTodoAsync(It.IsAny<Todo>()))
                .ReturnsAsync(createdTodo);

            // Act
            var result = await _controller.CreateTodo(todo);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult?.ActionName.Should().Be(nameof(_controller.GetTodo));
            createdAtActionResult?.RouteValues["id"].Should().Be(1);
            createdAtActionResult?.Value.Should().BeEquivalentTo(createdTodo);
        }

        [Fact]
        public async Task UpdateTodo_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Updated Task" };
            _mockService.Setup(service => service.GetTodoByIdAsync(1))
                .ReturnsAsync(todo);

            _mockService.Setup(service => service.UpdateTodoAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTodo(1, todo);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateTodo_WithInvalidId_ShouldReturnBadRequest()
        {
            // Arrange
            var todo = new Todo { Id = 2, Title = "Updated Task" };

            // Act
            var result = await _controller.UpdateTodo(1, todo);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteTodo_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Task 1" };
            _mockService.Setup(service => service.GetTodoByIdAsync(1))
                .ReturnsAsync(todo);

            _mockService.Setup(service => service.DeleteTodoAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTodo(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteTodo_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetTodoByIdAsync(999))
                .ReturnsAsync((Todo)null);

            // Act
            var result = await _controller.DeleteTodo(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}