using ToDo.Web.Models;

namespace ToDo.Web.Data;

public interface IToDoRepository
{
    Task<List<ToDoItem>> GetTasks();
    Task<ToDoItem> GetTaskByID(int taskID);
    void InsertTask(ToDoItem item);
    void DeleteTask(int taskID);

    Task RemoveAsync(int id);

    Task<ToDoItem> CreateAsync(ToDoItem task);
    Task UpdateAsync(ToDoItem task);
    Task<ToDoItem> GetByIdAsync(int id);
    void UpdateTask(ToDoItem task);
    void Save();
    IQueryable<ToDoItem> SetQueryable();
}