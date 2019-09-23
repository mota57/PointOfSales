using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata;
using SqlKata.Extensions;
using PointOfSales.Core.Service;

namespace PointOfSales.WebUI.Controllers
{
    public class ModifierDataTableConfig : VueTableConfig
    {
        public ModifierDataTableConfig()
        {
            TableName = nameof(Modifier);
            Fields.AddRange(new List<VueField>()
                {
                     new VueField(name:"Id", sqlField:"Modifier.Id" ),
                     new VueField(name:"Name", sqlField:"Modifier.Name"),
                     new VueField(name:"ModifierCount", sqlField:"ModifierCount"),
                });


            QueryBuilder = new Query("Modifier")
                     .Select("Modifier.Id", "Modifier.Name")
                     .ForSqlite(q => q.SelectRaw("(Select (COUNT(Name) || ' Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"))
                     .ForSqlServer(q => q.SelectRaw("(Select (COUNT(Name) + ' Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"));

        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ModifierController : ApplicationBaseController<Modifier>
    {
        private readonly IMapper _mapper;
        private readonly POSService _POSService;

        public ModifierController(POSContext context, IMapper mapper, POSService POSService)
            : base(context, new ModifierDataTableConfig())
        {
            _mapper = mapper;
            _POSService = POSService;
        }


        // GET: api/Modifier/5
        [HttpGet("{id}")]
        public ActionResult<Modifier> Get(int id)
        {

            var entity = _context.Modifier
                .Include(_ => _.ItemModifier)
                .AsNoTracking()
                .Where(_ => _.Id == id)
                .FirstOrDefault();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpsertProductModifiers(int productId, List<ProductModifier> productModifierClient)
        {
            if (_context.Product.FirstOrDefault(_ => _.Id == productId)  == null)
            {
                return NotFound();
            }

            await _POSService.UpsertDeleteProductModifiers(productId, productModifierClient);
            return Ok();
        }

        // POST: api/Modifier
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Post([FromBody] Modifier entity)
        {
            if (ModelState.IsValid)
            {
                _POSService.UpsertDeleteModiferAndItemModifier(entity);

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }

}
