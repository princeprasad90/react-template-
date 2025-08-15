using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetItems(int page = 1, int pageSize = 25, string search = "")
        {
            var allItems = Enumerable.Range(1, 100)
                .Select(i => new { id = i, name = $"Item {i}" });

            if (!string.IsNullOrWhiteSpace(search))
            {
                allItems = allItems.Where(i => i.name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var total = allItems.Count();
            var items = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { items, totalCount = total });
        }
    }
}
