using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;

namespace PointOfSales.WebUI.Controllers
{
   
    public class ApplicationBaseController<TEntity> : ControllerBase
        where TEntity : BaseEntity
    {

        protected readonly POSContext _context;

        public ApplicationBaseController(POSContext context)   {
            _context = context;
        }

        // GET: api/Entities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }



        [HttpGet("GetPickList/{word?}")]
        public async Task<IEnumerable<object>> GetPickList(string word = "")
        {
            return await _context.Set<TEntity>()
                .Where(_ => !string.IsNullOrEmpty(word) && EF.Functions.Like(_.Name, $"%{word}%"))
                .OrderBy(_ => _.Name)
                .Take(20)
                .Select(_ => new {  _.Name,  _.Id })
                .ToListAsync();
        }

        public bool EntityExists(int id)
        {
            return _context.Set<TEntity>().Any(e => e.Id == id);
        }



    }
}
