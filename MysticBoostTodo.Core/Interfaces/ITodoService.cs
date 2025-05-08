using System.Collections.Generic;
using System.Threading.Tasks;
using MysticBoostTodo.Core.Entities;

namespace MysticBoostTodo.Core.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<Todo> GetTodoByIdAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(int id);
    }
}