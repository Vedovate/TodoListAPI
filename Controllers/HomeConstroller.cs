using Microsoft.AspNetCore.Mvc;
using TodoTest.Data;
using TodoTest.Models;

namespace TodoTest.Controllers
{
    [ApiController]
    public class HomeConstroller : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get([FromServices] AppDbContext context) => Ok(context.Todos.ToList());
        

        [HttpGet("/{id:int}")]
        public IActionResult GetById(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var todos = context.Todos.FirstOrDefault(x => x.Id == id);
            if(todos == null)
                return NotFound();

            return Ok(todos);
        }

        [HttpPut("/{id:int}")]
        public IActionResult Put(
            [FromRoute] int id,
            [FromBody] TodoModel todo,
            [FromServices] AppDbContext context)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();

            model.Title = todo.Title;
            model.Done = todo.Done;

            context.Todos.Update(model);
            context.SaveChanges();
            return Ok(model);
        }


        [HttpPost("/")]
        public IActionResult Post(
            [FromBody] TodoModel todo,
            [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();
            return Created($"/{todo.Id}",todo);
        }


        [HttpDelete("/{id:int}")]
        public IActionResult Delete(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();

            context.Todos.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }
    }
}
