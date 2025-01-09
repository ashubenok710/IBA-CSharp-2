using ToDo.Web.DbContexts;
using ToDo.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ToDo.Web.Data;
public class ToDoRepository : IToDoRepository
{
    private DBToDoContext _context;

    public ToDoRepository()
    {
        this._context = new DBToDoContext();
    }

    public async Task<List<ToDoItem>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<ToDoItem> GetTaskByID(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public void InsertTask(ToDoItem task)
    {
        _context.Tasks.Add(task);
    }

    public void DeleteTask(int taskID)
    {
        ToDoItem task = _context.Tasks.Find(taskID);
        _context.Tasks.Remove(task);
    }

    public async Task RemoveAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public void UpdateTask(ToDoItem task)
    {
        _context.Entry(task).State = EntityState.Modified;
    }

    public void Save()
    {
        _context.SaveChangesAsync();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IQueryable<ToDoItem> SetQueryable()
    {
        return _context.Set<ToDoItem>().AsQueryable();
    }

    public async Task<ToDoItem> GetByIdAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        return task;
    }

    public async Task<ToDoItem> CreateAsync(ToDoItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task UpdateAsync(ToDoItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }
}