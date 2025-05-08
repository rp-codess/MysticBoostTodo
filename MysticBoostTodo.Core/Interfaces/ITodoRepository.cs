using System.Collections.Generic;
using System.Threading.Tasks;
using MysticBoostTodo.Core.Entities;

namespace MysticBoostTodo.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo> GetByIdAsync(int id);
        Task<Todo> AddAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(int id);
    }
}