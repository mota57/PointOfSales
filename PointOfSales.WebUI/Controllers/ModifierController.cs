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
            //if ( == null)
            //{
            //    return NotFound();
            //}
            await _POSService.UpsertProductModifiers(productId, productModifierClient);
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
                UpsertDeleteModiferAndItemModifier(_context, entity);

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [NonAction]
        public static void UpsertDeleteModiferAndItemModifier(POSContext context, Modifier modClient)
        {
            //load the modifier
            var modDb = context.Modifier
                .Include(_ => _.ItemModifier)
                .FirstOrDefault(_ => _.Id == modClient.Id);

            if(modDb == null)
            {
                context.Add(modClient);
            } else
            {
                //set values
                context.Entry(modDb).CurrentValues.SetValues(modClient);

                //check what are in the db and update it
                foreach (var item in modClient.ItemModifier)
                {
                    var itemDb = modDb.ItemModifier.FirstOrDefault(_ => _.Id == item.Id);

                    if (itemDb == null)
                    {
                        item.ModifierId = modClient.Id;
                        context.ItemModifier.Add(item);
                    }
                    else
                    {
                        context.Entry<ItemModifier>(itemDb).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in modDb.ItemModifier)
                {
                    if (!modClient.ItemModifier.Any(_ => _.Id == item.Id))
                    {
                        context.ItemModifier.Remove(item);
                    }

                }
            }

        }
    }

}
