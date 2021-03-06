using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.WebUI.Models;

namespace PointOfSales.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApplicationBaseController<Category>
    {

        public CategoryController(POSContext context, IMapper mapper) :
            base(context, new CategoryVueDataTableConfig(), mapper)
        {
        }


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Category>> Post([FromBody] Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
