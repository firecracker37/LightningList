using LightningList.Data;
using LightningList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightningList.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(AppDbContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoListViewModel model)
        {
            if (ModelState.IsValid)
            {
                var todoList = new TodoList { Name = model.Name };
                _context.Add(todoList);
                await _context.SaveChangesAsync();

                // Return a partial view with the new todo list
                return PartialView("_TodoListPartial", todoList);
            }

            // Handle error here (you can customize the error response as needed)
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(AddTodoITemViewModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
                var todoList = await _context.TodoLists
                             .Include(t => t.Items)
                             .FirstOrDefaultAsync(t => t.Id == id);

                if (todoList == null)
                {
                    return NotFound();
                }

                var todoItem = new TodoItem { Task = model.Task, TodoListId = id };
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                // Reload the todoList to get the updated list of items
                todoList = await _context.TodoLists
                                         .Include(t => t.Items)
                                         .FirstOrDefaultAsync(t => t.Id == id);

                // Return the updated todo list as a partial view
                return PartialView("_TodoListPartial", todoList);

            }

            // Handle error here
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(Guid id)
        {
            var task = await _context.TodoItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.IsComplete = !task.IsComplete;
            await _context.SaveChangesAsync();

            var todoList = await _context.TodoLists
                                         .Include(t => t.Items)
                                         .FirstOrDefaultAsync(t => t.Id == task.TodoListId);

            return PartialView("_TodoListPartial", todoList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.TodoItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(task);
            await _context.SaveChangesAsync();

            var todoList = await _context.TodoLists
                                         .Include(t => t.Items)
                                         .FirstOrDefaultAsync(t => t.Id == task.TodoListId);

            return PartialView("_TodoListPartial", todoList);
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            if (id.HasValue)
            {
                var todoList = await _context.TodoLists
                                             .Include(t => t.Items)
                                             .FirstOrDefaultAsync(t => t.Id == id.Value);

                if (todoList != null)
                {
                    return View(todoList);
                }
            }

            return RedirectToAction("Index", "Home"); // Send the user to the home page if there is no valid id
        }

    }
}
