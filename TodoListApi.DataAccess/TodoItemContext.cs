using Microsoft.EntityFrameworkCore;
using TodoListApi.DataAccess.Entities;

namespace TodoListApi.DataAccess
{
    public class TodoItemContext : DbContext
    {
        private string DBPath { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoItemContext()
        {
            DBPath = Path
                .Join(Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoListApi.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={DBPath}");
        }
    }
}
