using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingListApi
{
    [Authorize]
    [Route("api/[controller]")]
    public class ShoppingListController : Controller
    {
        private readonly ShoppingListContext _context;

        public ShoppingListController(ShoppingListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ShoppingListItem> GetAll()
        {
            return _context.ShoppingListItems.ToList();
        }

        [HttpGet("{id}", Name = "GetShoppingList")]
        public IActionResult GetById(long id)
        {
            var item = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ShoppingListItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.ShoppingListItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetShoppingList", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ShoppingListItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var shList = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (shList == null)
            {
                return NotFound();
            }
            shList.ItemID = item.ItemID;
            shList.ItemName = item.ItemName;
            shList.Price = item.Price;

            _context.ShoppingListItems.Update(shList);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var shList = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (shList == null)
            {
                return NotFound();
            }

            _context.ShoppingListItems.Remove(shList);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }

}