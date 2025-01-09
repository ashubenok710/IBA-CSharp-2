using Microsoft.EntityFrameworkCore;
using ToDo.Web.DbContexts;
using ToDo.Web.Models;

namespace ToDoApp.Web.Services
{
    public class ToDoService : IToDoService
    {
        private readonly DBToDoContext _appDbContext;

        public ToDoService(DBToDoContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<ToDoItem>> GetAllTasks()
        {
            return await _appDbContext.Tasks.ToListAsync();
        }

        public async Task<ToDoItem> GetTaskById(Guid id)
        {
            return await _appDbContext.Tasks.FindAsync(id);
        }

        public async Task<ToDoItem> AddTask(ToDoItem task)
        {
            _appDbContext.Tasks.Add(task);
            await _appDbContext.SaveChangesAsync();
            return task;
        }

        public async Task<ToDoItem> UpdateTask(ToDoItem task)
        {
            var existingTask = await _appDbContext.Tasks.FindAsync(task.Id);
            if (existingTask == null)
            {
                throw new ArgumentException("No task found with the given ID");
            }
            _appDbContext.Entry(existingTask).CurrentValues.SetValues(task);
            await _appDbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<ToDoItem> DeleteTask(Guid id)
        {
            var taskToRemove = await _appDbContext.Tasks.FindAsync(id);
            if (taskToRemove == null)
            {
                throw new ArgumentException("No task found with the given ID");
            }
            _appDbContext.Tasks.Remove(taskToRemove);
            await _appDbContext.SaveChangesAsync();
            return taskToRemove;
        }



    }
}
