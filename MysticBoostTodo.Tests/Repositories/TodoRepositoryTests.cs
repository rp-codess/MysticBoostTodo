// Repositories/TodoRepositoryTests.cs
using FluentAssertions;
using MysticBoostTodo.Core.Entities;
using MysticBoostTodo.Infrastructure.Repositories;
using MysticBoostTodo.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace MysticBoostTodo.Tests.Repositories
{
    public class TodoRepositoryTests : RepositoryTestBase
    {
        private readonly TodoRepository _repository;

        public TodoRepositoryTests()
        {
            _repository = new TodoRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var todo1 = new Todo { Title = "Test Todo 1", Description = "Description 1" };
            var todo2 = new Todo { Title = "Test Todo 2", Description = "Description 2" };
            _context.Todos.Add(todo1);
            _context.Todos.Add(todo2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Title == "Test Todo 1");
            result.Should().Contain(t => t.Title == "Test Todo 2");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTodo_WhenTodoExists()
        {
            // Arrange
            var todo = new Todo { Title = "Test Todo", Description = "Description" };
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(todo.Id);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Todo");
            result.Description.Should().Be("Description");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewTodo()
        {
            // Arrange
            var todo = new Todo { Title = "New Todo", Description = "New Description" };

            // Act
            var result = await _repository.AddAsync(todo);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);

            // Verify it's in the database
            var todoFromDb = await _context.Todos.FindAsync(result.Id);
            todoFromDb.Should().NotBeNull();
            todoFromDb.Title.Should().Be("New Todo");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingTodo()
        {
            // Arrange
            var todo = new Todo { Title = "Test Todo", Description = "Description" };
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            // Update the todo
            todo.Title = "Updated Title";
            todo.Description = "Updated Description";

            // Act
            await _repository.UpdateAsync(todo);

            // Assert
            var updatedTodo = await _context.Todos.FindAsync(todo.Id);
            updatedTodo.Should().NotBeNull();
            updatedTodo.Title.Should().Be("Updated Title");
            updatedTodo.Description.Should().Be("Updated Description");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTodo()
        {
            // Arrange
            var todo = new Todo { Title = "Test Todo", Description = "Description" };
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(todo.Id);

            // Assert
            var todoFromDb = await _context.Todos.FindAsync(todo.Id);
            todoFromDb.Should().BeNull();
        }
    }
}