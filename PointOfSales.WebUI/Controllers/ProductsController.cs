using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PointOfSales.WebUI.Extensions;

namespace PointOfSales.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApplicationBaseController<Product>
    {
        private readonly IMapper _mapper;

        public ProductsController(POSContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromForm] int id, [FromBody] Product product)
        {
            if (id != product.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(product).State = EntityState.Modified;

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

        // POST: api/Products
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);
            product.MainImage = await dto.Image.ToBytes();


            if (ModelState.IsValid)
            {

                _context.Products.Add(product);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

    }
}
