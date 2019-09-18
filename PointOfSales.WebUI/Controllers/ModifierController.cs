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

namespace PointOfSales.WebUI.Controllers
{
    public class ModifierDataTableConfig : VueTableConfig
    {
        public ModifierDataTableConfig()
        {
            TableName = nameof(Modifier);
            Fields.AddRange(new List<VueField>()
                {
                     new VueField(name:"Id" ),
                     new VueField(name:"Name"),
                     new VueField(name:"ModifierCount"),
                });


            QueryBuilder = new Query("Modifier")
                     .Select("Modifier.Id", "Modifier.Name")
                     .ForSqlite(q => q.SelectRaw("(Select (COUNT(Name) || 'Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"))
                     .ForSqlServer(q => q.SelectRaw("(Select (COUNT(Name) + 'Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount"));

        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ModifierController : ApplicationBaseController<Modifier>
    {
        private readonly IMapper _mapper;

        public ModifierController(POSContext context, IMapper mapper)
            : base(context, new ModifierDataTableConfig())
        {
            _mapper = mapper;

        }


        // GET: api/Modifier/5
        [HttpGet("{id}")]
        public ActionResult<ModifierDTO> Get(int id)
        {

            var entity = _context.Product
                .AsNoTracking()
                .Where(_ => _.Id == id)
                .FirstOrDefault();

            var dto = _mapper.Map<ModifierDTO>(entity);


            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
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
