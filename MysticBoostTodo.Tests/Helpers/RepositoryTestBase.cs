// Helpers/RepositoryTestBase.cs
using Microsoft.EntityFrameworkCore;
using MysticBoostTodo.Infrastructure.Data;
using System;

namespace MysticBoostTodo.Tests.Helpers
{
    public abstract class RepositoryTestBase : IDisposable
    {
        protected readonly AppDbContext _context;

        public RepositoryTestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}