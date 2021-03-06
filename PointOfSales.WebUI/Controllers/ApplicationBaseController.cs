using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        protected readonly IMapper _mapper;

        protected readonly POSContext _context;

        public ApplicationBaseController(POSContext context, VueTableConfig tableConfig, IMapper mapper)   {
            _context = context;
            _mapper = mapper;
            TableConfig = tableConfig;
            
        }


        // GET: api/Entity/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }




        // GET: api/Entities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }


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

        /// <summary>
        /// This function handle the data for the datatable.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GetDatatable")]
        public async Task<Dictionary<string, object>> GetDataTable([FromQuery] VueTableParameters parameters)
        {
            IVueTablesInterface reader = new VueTableReader();
            var result = await reader.GetAsync(TableConfig, parameters);
            return result;
        }

        /// <summary>
        /// This function return the vueTableConfig to create the table columns
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTableMetadata")]
        public object GetTableMetadata()
        {
            return new VueTableConfigDTO(TableConfig);
        }

        [NonAction]
        public bool EntityExists(int id)
        {
            return _context.Set<TEntity>().Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {

            var set = _context.Set<TEntity>();
            var entity = await set.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            set.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [NonAction]
         public TResult GetService<TResult>() {
            var services = HttpContext.RequestServices;
            TResult result = (TResult) services.GetService(typeof(TResult));
            return result;
        }

    }
}
