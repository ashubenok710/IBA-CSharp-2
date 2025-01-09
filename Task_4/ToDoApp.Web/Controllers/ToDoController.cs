using ToDo.Web.Data;
using ToDo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ToDo.Web.Controllers;

//[Route("[controller]")]
//[ApiController]
public class ToDoController : Controller
{

    private readonly IToDoRepository _toDoRepository;

    private readonly IUserRepository _userRepository;

    public ToDoController(IToDoRepository taskRepository)
    {
        _toDoRepository = taskRepository;
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetData()
    {
        Console.WriteLine(_userRepository);


        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        int skip = start != null ? Convert.ToInt32(start) : 0;
        int recordsTotal = 0;

        var taskItems = _toDoRepository.SetQueryable();

        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        {
            taskItems = taskItems.OrderBy(sortColumn + " " + sortColumnDirection);
        }
        if (!string.IsNullOrEmpty(searchValue))
        {
            taskItems = taskItems.Where(m => m.Name.Contains(searchValue)
                                        || m.Priority.Contains(searchValue)
                                        || m.DueDate.Equals(searchValue));
        }

        recordsTotal = await taskItems.CountAsync();

        var jsonData = new
        {
            draw = draw,
            recordsFiltered = recordsTotal,
            recordsTotal = recordsTotal,
            data = await taskItems
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync()
        };


        return Ok(jsonData);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<List<ToDoItem>>> GetTasks()
    {
        return Ok(await _toDoRepository.GetTasks());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public void InsertTask(ToDoItem task)
    {
        _toDoRepository.InsertTask(task);
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<List<ToDoItem>>> UpdateTask(ToDoItem task, int id)
    {
        var dbTask = await _toDoRepository.GetTaskByID(id);
        
        if (dbTask == null)
            return NotFound("No task here. :/");

        dbTask.Name = task.Name;
        dbTask.Priority = task.Priority;
        dbTask.DueDate = task.DueDate;
        dbTask.IsCompleted = task.IsCompleted;

        _toDoRepository.Save();
        //_context.SaveChangesAsync();
        return Ok(await _toDoRepository.GetTasks());
    }

    [HttpDelete("{id}")]
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<List<ToDoItem>>> DeleteTask(int id)
    {
        var dbTask = await _toDoRepository.GetTaskByID(id);
        if (dbTask == null)
            return NotFound("No task here. :/");

        _toDoRepository.DeleteTask(dbTask.Id);

        return Ok(await _toDoRepository.GetTasks());
    }

    [HttpPost, ActionName("Delete")]
    //[Authorize]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _toDoRepository.RemoveAsync(id);
        return Json(new { success = true });
    }

    public async Task<IActionResult> AddOrEdit(int id = 0)
    {

        if (id == 0)
        {
            return View(new ToDoItem());
        }
        else
        {
            return View(await _toDoRepository.GetByIdAsync(id));
        }
    }

    [HttpPost]
    //[Authorize]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEdit(ToDoItem task)
    {
        if (task.Id == 0)
        {         
            task.EmployeeID = 1;
            await _toDoRepository.CreateAsync(task);

            TempData["AlertMessage"] = "Created Successfully";
        }
        else
        {
            await _toDoRepository.UpdateAsync(task);
            TempData["AlertMessage"] = "Updated Successfully";
        }

        return Json(new { success = true });
    }


    [HttpPut, ActionName("Update")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromBody] ToDoItem task, int id)
    {
        var dbTask = await _toDoRepository.GetTaskByID(id);


        if (dbTask == null)
            return NotFound("No task here. :/");

        dbTask.Name = task.Name;
        dbTask.Priority = task.Priority;
        dbTask.DueDate = task.DueDate;
        dbTask.IsCompleted = task.IsCompleted;

        _toDoRepository.Save();
        //_context.SaveChangesAsync();
        return Json(new { success = true });
    }

}