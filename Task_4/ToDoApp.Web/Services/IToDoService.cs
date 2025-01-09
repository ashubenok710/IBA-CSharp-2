using ToDo.Web.Models;

namespace ToDoApp.Web.Services
{
    public interface IToDoService
    {
        Task<List<ToDoItem>> GetAllTasks();
        Task<ToDoItem> GetTaskById(Guid id);
        Task<ToDoItem> AddTask(ToDoItem task);
        Task<ToDoItem> UpdateTask(ToDoItem task);
        Task<ToDoItem> DeleteTask(Guid id);
    }
}
