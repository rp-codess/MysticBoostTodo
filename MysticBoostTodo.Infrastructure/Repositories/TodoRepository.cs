using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MysticBoostTodo.Core.Entities;
using MysticBoostTodo.Core.Interfaces;
using MysticBoostTodo.Infrastructure.Data;

namespace MysticBoostTodo.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<Todo> AddAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task UpdateAsync(Todo todo)
        {
            // Instead of setting EntityState.Modified directly (which causes the tracking error)
            // First fetch the entity from the database
            var existingTodo = await _context.Todos.FindAsync(todo.Id);
            if (existingTodo != null)
            {
                // Update the properties manually
                _context.Entry(existingTodo).CurrentValues.SetValues(todo);
                // Save changes
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }
    }
}