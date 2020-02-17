using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Service;
using PointOfSales.WebUI.Models;

namespace PointOfSales.WebUI.Controllers
{

    [Route ("api/[controller]")]
    [ApiController]
    public partial class ProductController : ApplicationBaseController<Product> {
        private readonly POSService _POSService;
        private readonly ProductService _productService;
        private readonly ProductContentProvider productContent;
        public ProductController (POSContext context, IMapper mapper, POSService pOSService) : base (context, new ProductDataTableConfig (), mapper) {
            _POSService = pOSService;
            _productService = new ProductService (context);
            productContent = new ProductContentProvider (this.GetService<IHostingEnvironment> ());

        }

        // GET: api/Categories/5
        [HttpGet ("{id}")]
        public async override Task<ActionResult> Get (int id) {
            var product = await _productService.GetProduct (id);

            if (product == null) {
                return NotFound ();
            }

            var dto = _mapper.Map<ProductFormDTO> (product);

            return Ok (dto);
        }

        // PUT: api/Products/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> PutProduct (int id, [FromForm] ProductFormDTO dto) {

            if (id != dto.Id || !ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            try {
                Product product = await _context.Product
                    .Include (_ => _.ProductModifier)
                    .FirstOrDefaultAsync (_ => _.Id == id);

                _mapper.Map (dto, product);

                _context.Entry (product).State = EntityState.Modified;

                _productService.UpsertDeleteProductModifiers (product, dto);

                await SaveProduct (product, dto);

            } catch (DbUpdateConcurrencyException) {
                if (!EntityExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        // POST: api/Products
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Product>> PostProduct ([FromForm] ProductFormDTO dto) {

            if (ModelState.IsValid) {
                Product product = _mapper.Map<Product> (dto);

                this._context.Product.Add (product);

                await SaveProduct (product, dto);

                return CreatedAtAction (nameof (PostProduct), new { id = product.Id }, product);
            } else {
                return BadRequest (ModelState);
            }
        }

       

        private async Task SaveProduct (Product product, ProductFormDTO dto) {

            await _context.SaveChangesAsync ()
                .ContinueWith (async (task) => {
                    await productContent.HandleFile (product, dto);
                });
        }

    }

    public class ProductContentProvider {
        private FileContentProvider fileContentManager;
        public ProductContentProvider (IHostingEnvironment webHost) {
            fileContentManager = new FileContentProvider (webHost, "product");
        }
        public async Task HandleFile (Product product, ProductFormDTO dto) {
            if (dto.ImageDeleted) {
                new Thread (() => fileContentManager
                .RemoveFileFromDisk (new string[] { product.MainImage }))
                .Start ();
                product.MainImage = null;
            } else if (dto.MainImageForm != null) {
                //save to disk
                var fileNames = await fileContentManager.SaveImages (new IFormFile[] { dto.MainImageForm });
                product.MainImage = fileNames[0];
            }
        }
    }
}