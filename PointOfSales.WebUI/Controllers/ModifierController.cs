using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Service;
using PointOfSales.WebUI.Models;

namespace PointOfSales.WebUI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ModifierController : ApplicationBaseController<Modifier>
    {
        private readonly POSService _POSService;
        private readonly ModifierService _modifierService;

        public ModifierController(POSContext context, IMapper mapper, POSService POSService)
            : base(context, new ModifierDataTableConfig(), mapper)
        {
            _POSService = POSService;
            _modifierService  = new ModifierService(context);
        }


        // GET: api/Modifier/5
        [HttpGet("{id}")]
        public async override  Task<ActionResult> Get(int id)
        {

            var entity = await _context.Modifier
                .Include(_ => _.ItemModifier)
                .AsNoTracking()
                .Where(_ => _.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpsertProductModifiers(int productId, List<ProductModifier> productModifierClient)
        {
            if (!_context.Set<Product>().Any(_ => _.Id == productId))
            {
                return NotFound();
            }
            var productService = new ProductService(_context);
            var product = await productService.GetProduct(productId);

            productService.UpsertDeleteProductModifiers(product, productModifierClient);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// I can create and update modifiers with this function
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Post([FromBody] Modifier entity)
        {
            if (ModelState.IsValid)
            {
                _modifierService.UpsertDeleteModiferAndItemModifier(entity);

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
