using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.DTO;
using AutoMapper;
using PointOfSales.WebUI.Extensions;

using PointOfSales.Core.Service;
using PointOfSales.Core.Infraestructure;
using PointOfSales.WebUI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace PointOfSales.WebUI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public partial class ProductController : ApplicationBaseController<Product>
    {
        private readonly POSService _POSService;
        private readonly ProductService _productService;

        public ProductController(POSContext context, IMapper mapper, POSService pOSService)
            : base(context, new ProductDataTableConfig(), mapper)
        {
            _POSService = pOSService;
            _productService = new ProductService(context);
        }


        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async override Task<ActionResult> Get(int id)
        {
            var product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ProductFormDTO>(product);

            return Ok(dto);
        }



        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductFormDTO dto,
              [FromServices] IHostingEnvironment webHost)
        {
            FileContentProvider fileContentManager = new FileContentProvider(webHost, "product");

            if (id != dto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product product = await _context.Product
                                        .Include(_ => _.ProductModifier)
                                        .FirstOrDefaultAsync(_ => _.Id == id);
                await HandleFile(dto, fileContentManager, product);

                _mapper.Map(dto, product);

                _context.Entry(product).State = EntityState.Modified;

                var productModifierClient = ProductMapper.CreateListOfProductModifier(dto);

                _productService.UpsertDeleteProductModifiers(product, productModifierClient);

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

        private static async Task HandleFile(ProductFormDTO dto, FileContentProvider fileContentManager, Product product )
        {
            if (dto.ImageDeleted)
            {
                Thread task = new Thread(() => fileContentManager.RemoveFileFromDisk(new string[] { product.MainImage }));
                task.Start();
                product.MainImage = null;
            }
            else if (dto.MainImageForm != null)
            {
                //save to disk
                var fileNames = await fileContentManager.SaveImages(new IFormFile[] { dto.MainImageForm });
                product.MainImage = fileNames[0];
            }
        }

        // POST: api/Products
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductFormDTO dto, [FromServices] IHostingEnvironment webHost)
        {
            FileContentProvider fileContentManager = new FileContentProvider(webHost, "product");

            if (ModelState.IsValid)
            {
                Product product = _mapper.Map<Product>(dto);
                await HandleFile(dto, fileContentManager, product);

                _context.Product.Add(product);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
