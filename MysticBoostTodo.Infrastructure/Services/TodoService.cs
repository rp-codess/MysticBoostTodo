using System.Collections.Generic;
using System.Threading.Tasks;
using MysticBoostTodo.Core.Entities;
using MysticBoostTodo.Core.Interfaces;

namespace MysticBoostTodo.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _todoRepository.GetAllAsync();
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _todoRepository.GetByIdAsync(id);
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            return await _todoRepository.AddAsync(todo);
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            await _todoRepository.UpdateAsync(todo);
        }

        public async Task DeleteTodoAsync(int id)
        {
            await _todoRepository.DeleteAsync(id);
        }
    }
}