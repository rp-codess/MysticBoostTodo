// Services/TodoServiceTests.cs
using FluentAssertions;
using Moq;
using MysticBoostTodo.Core.Entities;
using MysticBoostTodo.Core.Interfaces;
using MysticBoostTodo.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MysticBoostTodo.Tests.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockRepository;
        private readonly TodoService _service;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITodoRepository>();
            _service = new TodoService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllTodosAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var todos = new List<Todo>
            {
                new Todo { Id = 1, Title = "Task 1" },
                new Todo { Id = 2, Title = "Task 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(todos);

            // Act
            var result = await _service.GetAllTodosAsync();

            // Assert
            result.Should().BeEquivalentTo(todos);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTodo_WhenTodoExists()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Task 1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(todo);

            // Act
            var result = await _service.GetTodoByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(todo);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Todo)null);

            // Act
            var result = await _service.GetTodoByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateTodoAsync_ShouldCallRepository()
        {
            // Arrange
            var todo = new Todo { Title = "New Task" };
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Todo>()))
                .ReturnsAsync(todo);

            // Act
            var result = await _service.CreateTodoAsync(todo);

            // Assert
            result.Should().BeEquivalentTo(todo);
            _mockRepository.Verify(repo => repo.AddAsync(todo), Times.Once);
        }

        [Fact]
        public async Task UpdateTodoAsync_ShouldCallRepository()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Updated Task" };
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Todo>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateTodoAsync(todo);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(todo), Times.Once);
        }

        [Fact]
        public async Task DeleteTodoAsync_ShouldCallRepository()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteTodoAsync(1);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}