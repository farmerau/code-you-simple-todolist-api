using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.DataAccess;
using TodoListApi.DataAccess.Entities;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private TodoItemContext _dbContext;

        public TodoController(TodoItemContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        [HttpPut("items")]
        public async Task<ActionResult<TodoItemModel>> Add(TodoItemModel item, CancellationToken token)
        {
            TodoItem itemToSave = new()
            {
                Id = item.Id,
                Title = item.Title,
                IsComplete = item.IsComplete
            };

            await _dbContext.TodoItems.AddAsync(itemToSave, token);
            await _dbContext.SaveChangesAsync(token);

            item.Id = itemToSave.Id;

            return Ok(item);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<TodoItemModel>> Get(Guid id, CancellationToken token)
        {
            TodoItem? item = await _dbContext.TodoItems.FindAsync(new object[] { id }, token);

            if (item == null)
            {
                return NotFound();
            }

            TodoItemModel itemToReturn = new()
            {
                Id = item.Id,
                Title = item.Title,
                IsComplete = item.IsComplete
            };

            return Ok(itemToReturn);
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetAll(bool includeCompleted = false, CancellationToken token = default)
        {
            IQueryable<TodoItem> todoItems = _dbContext.TodoItems;

            if (!includeCompleted)
            {
                todoItems = todoItems.Where(item => !item.IsComplete);  
            }
            
            IEnumerable<TodoItemModel> itemsToReturn = await todoItems
                .Select(item => new TodoItemModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    IsComplete = item.IsComplete
                })
                .ToListAsync(token);

            return Ok(itemsToReturn);
        }

        [HttpPatch("items/{id}")]
        public async Task<ActionResult<TodoItemModel>> Update(TodoItemModel item)
        {
            TodoItem? itemToUpdate = await _dbContext.TodoItems.FindAsync(item.Id);

            if (itemToUpdate is null)
            {
                return NotFound();
            }

            itemToUpdate.Title = item.Title;
            itemToUpdate.IsComplete = item.IsComplete;
            await _dbContext.SaveChangesAsync();

            return new TodoItemModel
            {
                Id = itemToUpdate.Id,
                Title = itemToUpdate.Title,
                IsComplete = itemToUpdate.IsComplete
            };
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            TodoItem? itemToUpdate = await _dbContext.TodoItems.FindAsync(id);

            if (itemToUpdate is null)
            {
                return NotFound();
            }

            _dbContext.TodoItems.Remove(itemToUpdate);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
