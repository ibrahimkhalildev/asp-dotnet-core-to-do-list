using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("tasks")] // Custom route: URL will start with /tasks
    public class TodoController : ControllerBase
    {
        // In-memory To-Do list (no database)
        private static List<TodoItem> todos = new List<TodoItem>
        {
            new TodoItem
            {
                Id = 1,
                Title = "Learn ASP.NET Web API",
                Description = "Study basic Web API concepts and test endpoints using Swagger",
                IsCompleted = false,
                CreatedAt = DateTime.Now
            },
            new TodoItem
            {
                Id = 2,
                Title = "Digital Marketing on Linked In",
                Description = "Study basic Web API of Marketing in Web Developmen",
                IsCompleted = false,
                CreatedAt = DateTime.Now
            }
        };

        // GET: /tasks
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll()
        {
            return Ok(todos);
        }

        // GET: /tasks/{id}
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
                return NotFound(new { Message = $"No task found with ID {id}." });

            return Ok(item);
        }

        // POST: /tasks
        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem newItem)
        {
            newItem.Id = todos.Count > 0 ? todos.Max(t => t.Id) + 1 : 1;
            newItem.CreatedAt = DateTime.Now;

            if (newItem.IsCompleted)
                newItem.CompletedAt = DateTime.Now;

            todos.Add(newItem);

            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        // PUT: /tasks/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem updatedItem)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
                return NotFound(new { Message = $"No task found with ID {id} to update." });

            item.Title = updatedItem.Title;
            item.Description = updatedItem.Description;
            item.IsCompleted = updatedItem.IsCompleted;

            if (updatedItem.IsCompleted)
                item.CompletedAt = DateTime.Now;
            else
                item.CompletedAt = null;

            return NoContent();
        }

        // DELETE: /tasks/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
                return NotFound(new { Message = $"No task found with ID {id} to delete." });

            todos.Remove(item);
            return NoContent();
        }
    }
}
