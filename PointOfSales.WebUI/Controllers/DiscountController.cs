using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.WebUI.Models;

namespace PointOfSales.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ApplicationBaseController<Discount>
    {
        public DiscountController(POSContext context, IMapper mapper)
            : base(context, new DiscountVueDataTableConfig(), mapper)
        {
        }


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Discount discount)
        {
            if (id != discount.Id)
            {
                return BadRequest();
            }

            _context.Entry(discount).State = EntityState.Modified;

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
        public async Task<ActionResult> Post([FromBody] Discount discount)
        {
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
 }
