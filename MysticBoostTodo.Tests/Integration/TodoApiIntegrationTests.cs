// Integration/TodoApiIntegrationTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MysticBoostTodo.Api;
using MysticBoostTodo.Core.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MysticBoostTodo.Tests.Integration
{
    public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(); // Fixed syntax error here
        }

        [Fact]
        public async Task GetTodos_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/todos");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateTodo_ShouldReturnCreatedTodo()
        {
            // Arrange
            var newTodo = new Todo
            {
                Title = "Integration Test Todo",
                Description = "Created during integration test"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/todos", newTodo);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var returnedTodo = await response.Content.ReadFromJsonAsync<Todo>();
            returnedTodo.Should().NotBeNull();
            returnedTodo?.Title.Should().Be("Integration Test Todo");
        }
    }
}