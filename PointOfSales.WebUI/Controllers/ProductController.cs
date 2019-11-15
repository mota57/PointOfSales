using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using PointOfSales.Core.DTO;
using AutoMapper;
using PointOfSales.WebUI.Extensions;
using PointOfSales.Core.Infraestructure.VueTable;
using System.Linq;
using SqlKata;
using System;
using Newtonsoft.Json;
using PointOfSales.Core.Service;
using PointOfSales.Core.Infraestructure;

namespace PointOfSales.WebUI.Controllers
{
    public class ProductDataTableConfig : VueTableConfig
    {
        public ProductDataTableConfig()
        : base(
                nameof(Product),
                new List<VueField>()
                {
                     new VueField(name:"Id", sqlField:"Product.Id"),
                     new VueField(name:"Name", sqlField:"Product.Name"),
                     new VueField(name:"Price", sqlField:"Product.Price"),
                     new VueField(name:"ProductCode", sqlField:"Product.ProductCode"),
                     new VueField(name:"CategoryName", sqlField:"Category.Name")
                },
                new Query(nameof(Product))
                .LeftJoin("Category", "Category.Id", "Product.CategoryId")
                .Select("Product.{Id,Name,Price,ProductCode}", "Category.Name as CategoryName")
        )
        {

        }
    }




    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApplicationBaseController<Product>
    {
        private readonly POSService _POSService;
        private readonly ProductService _productService;

        public ProductsController(POSContext context, IMapper mapper, POSService pOSService)
            : base(context, new ProductDataTableConfig(), mapper)
        {
            _POSService = pOSService;
            _productService = new ProductService(context);
        }


        // GET: api/Categories/5
        [HttpGet("{id}")]
        public  async override Task<ActionResult> Get(int id)
        {
            var product =  await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }
            
            var dto = _mapper.Map<ProductFormDTO>(product);

            return Ok(dto);
        }



        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductFormDTO dto)
        {

            if (id != dto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product product = await _context.Product
                                        .Include(_ => _.ProductModifier)
                                        .FirstOrDefaultAsync(_ => _.Id == id);

            
                if (dto.ImageDeleted)
                {
                    product.MainImage = null;
                } else if (dto.MainImageForm != null)
                {
                    product.MainImage = await dto.MainImageForm.ToBytes();
                }


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

        // POST: api/Products
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductFormDTO dto)
        {            

            if (ModelState.IsValid)
            {
                Product product = _mapper.Map<Product>(dto);
                product.MainImage = await dto.MainImageForm.ToBytes();

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
