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


        // PUT: api/Modifier/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ModifierDTO dto)
        {

            if (id != dto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Modifier entity = await _context.Modifier.FirstOrDefaultAsync(_ => _.Id == id);
            if (entity == null) return BadRequest();

            _mapper.Map(dto, entity);
            _context.Entry(entity).State = EntityState.Modified;

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

        // POST: api/Modifier
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Post([FromBody] ModifierDTO dto)
        {
            Modifier entity = _mapper.Map<Modifier>(dto);


            if (ModelState.IsValid)
            {
                _context.Modifier.Add(entity);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ItemModifierController : ApplicationBaseController<ItemModifier>
    {
        private readonly IMapper _mapper;

        public ItemModifierController(POSContext context, IMapper mapper)
            : base(context, new ModifierDataTableConfig())
        {
            _mapper = mapper;

        }


        // PUT: api/Modifier/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]  List<ItemModifier> dtos)
        {

            //if (id != dto.Id || !ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            Modifier entity = await _context.Modifier.Include(_ => _.ItemModifier).FirstOrDefaultAsync(_ => _.Id == id);
            if (entity == null) return BadRequest();

            _context.Entry(entity).State = EntityState.Modified;

            foreach(var item in entity.ItemModifier)
            {
                foreach(var im in dtos)
                {
                    if(item.Id == im.Id)
                    {
                        //map

                    }

                }
            }

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

        // POST: api/Modifier
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Post([FromBody] List<ItemModifierDTO> dtos)
        {
            Modifier modifier = _context.Modifier.FirstOrDefault(_ => dtos.Any(d => d.Id == _.Id));

            List<ItemModifier> entities = _mapper.Map<List<ItemModifier>>(dtos);

            if (ModelState.IsValid)
            {
                foreach(var item in entities)
                {
                    modifier.ItemModifier.Add(item);
                }

                await _context.SaveChangesAsync();

                return StatusCode(201);
             }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
