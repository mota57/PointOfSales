using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;

namespace PointOfSales.WebUI.Controllers
{
    public class CategoryVueDataTableConfig : VueTableConfig
        {
            public CategoryVueDataTableConfig()
            {
                TableName = nameof(Category);
                Fields.AddRange(new List<VueField>()
                {
                     new VueField("Id", false),
                     new VueField("Name")
                });

            }
        }


    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ApplicationBaseController<Category>
    {

        public CategoriesController(POSContext context): base(context, new CategoryVueDataTableConfig())
        {
        }


       


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
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
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}
