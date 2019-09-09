using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;

namespace PointOfSales.WebUI.Controllers
{
   
    public class ApplicationBaseController<TEntity> : ControllerBase
        where TEntity : BaseEntity
    {
        protected VueTableConfig TableConfig { get; set; }

        protected readonly POSContext _context;

        public ApplicationBaseController(POSContext context, VueTableConfig tableConfig)   {
            _context = context;
            TableConfig = tableConfig;
        }

        // GET: api/Entities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        //GET: api/Categories/5
        //public abstract async Task<ActionResult<TEntity>> Get(int id);
        //{
        //    var entity = await _context.Set<TEntity>().FindAsync(id);

        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    return entity;
        //}



        [HttpGet("GetPickList/{word?}")]
        public async Task<IEnumerable<object>> GetPickList(string word = "")
        {
            var query = _context.Set<TEntity>()
               .OrderBy(_ => _.Name)
               .Take(20);

            if (!string.IsNullOrEmpty(word))
                query = query.Where(_ => EF.Functions.Like(_.Name, $"%{word}%"));

            return await  query.Select(_ => new {  _.Name,  _.Id }).ToListAsync();
        }

        [HttpGet("GetDatatable")]
        public async Task<Dictionary<string, object>> GetDataTable([FromQuery] VueTableParameters parameters)
        {
            VueTableReader reader = new VueTableReader();
            var result = await reader.GetAsync(TableConfig, parameters);
            return result;
        }

        [HttpGet("GetTableMetadata")]
        public object GetTableMetadata()
        {
            return TableConfig;
        }

        [NonAction]
        public bool EntityExists(int id)
        {
            return _context.Set<TEntity>().Any(e => e.Id == id);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TEntity>> DeleteProduct(int id)
        {
            var set = _context.Set<TEntity>();
            var entity = await set.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            set.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }



    }
}
