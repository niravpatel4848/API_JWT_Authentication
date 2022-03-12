using API_JWT_Authentication.Context;
using API_JWT_Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API_JWT_Authentication.Controllers
{
    //[Authorize]
    [Route("api/[controller]")] 
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            var items = _context.Items.ToList();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult CreateItem(Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                _context.SaveChanges();

                return CreatedAtAction("GetItem", new { item.Id }, item);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            var existItem = _context.Items.FirstOrDefault(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            existItem.Title = item.Title;
            existItem.Description = item.Description;
            existItem.Done = item.Done;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            var existItem = _context.Items.FirstOrDefault(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            _context.Items.Remove(existItem);
            _context.SaveChanges();

            return Ok(existItem);
        }
    }
}
