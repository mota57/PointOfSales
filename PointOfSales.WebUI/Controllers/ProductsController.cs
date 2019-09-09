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
using System;
using Newtonsoft.Json;

namespace PointOfSales.WebUI.Controllers
{
    public class ProductDataTableConfig : VueTableConfig
    {
        public ProductDataTableConfig()
        {
            var prd = new Product();
            TableName = "Products";
            Fields.AddRange(new List<VueField>()
                {
                     new VueField(nameof(prd.Id), false),
                     new VueField(nameof(prd.Name)),
                     new VueField(nameof(prd.Price)),
                     new VueField(nameof(prd.ProductCode)),
                });

        }
    }




    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApplicationBaseController<Product>
    {
        private readonly IMapper _mapper;

        public ProductsController(POSContext context, IMapper mapper)
            : base(context, new ProductDataTableConfig())
        {
            _mapper = mapper;

        }


        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {

            var entity = _context.Products.Include(_ => _.Categories).FirstOrDefault(_ => _.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            entity.MainImage = null;

            return JsonConvert.SerializeObject(entity);

        }

        [HttpGet("GetImage")]
        public FileResult GetImage(int id)
        {
            var entity = _context.Products.Include(_ => _.Categories).FirstOrDefault(_ => _.Id == id);
            return File(entity.MainImage, "image/png");
        }



        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromForm] int id, [FromForm] ProductDTO dto)
        {

            if (id != dto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await _context.Products.FirstOrDefaultAsync(_ => _.Id == id);
            if (product == null) return BadRequest();


            if (!this.Request.Form.ContainsKey(nameof(dto.Image)))
            {
                product.MainImage = await dto.Image.ToBytes();
            }
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.ProductCode = dto.ProductCode;
            if(dto.CategoryId.HasValue)
            {
                var category = _context.Categories.Find(dto.CategoryId.Value);
                product.Categories.Add(category);
            }

            _context.Entry(product).State = EntityState.Modified;

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

        // POST: api/Products
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);
            product.MainImage = await dto.Image.ToBytes();


            if (ModelState.IsValid)
            {

                _context.Products.Add(product);

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
