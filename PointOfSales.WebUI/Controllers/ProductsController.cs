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

namespace PointOfSales.WebUI.Controllers
{
    public class ProductDataTableConfig : VueTableConfig
    {
        public ProductDataTableConfig()
        {
            TableName = nameof(Product);
            Fields.AddRange(new List<VueField>()
                {
                     new VueField(name:"Id", sqlField:"Product.Id"),
                     new VueField(name:"Name", sqlField:"Product.Name"),
                     new VueField(name:"Price", sqlField:"Product.Price"),
                     new VueField(name:"ProductCode", sqlField:"Product.ProductCode"),
                     new VueField(name:"CategoryName", sqlField:"Category.Name")
                });

            QueryBuilder = new Query(TableName)
                          .LeftJoin("Category", "Category.Id", "Product.CategoryId")
                          .Select("Product.{Id,Name,Price,ProductCode}", "Category.Name as CategoryName");

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
        public ActionResult<ProductFormDTO> Get(int id)
        {

            var dto = _context.Product
                .AsNoTracking()
                .Where(_ => _.Id == id)
                .Select(_ =>
                     new ProductFormDTO
                     {
                         Id = _.Id,
                         MainImage = _.MainImage,
                         ProductCode = _.ProductCode,
                         Name = _.Name,
                         Price = _.Price,
                         CategoryId =  _.CategoryId
                     })
                .FirstOrDefault();


            if (dto == null)
            {
                return NotFound();
            }
            dto.AttributeIds = new List<int>() { 2,3 };

            return dto;
        }


        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductFormDTO dto)
        {

            if (id != dto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await _context.Product.FirstOrDefaultAsync(_ => _.Id == id);
            if (product == null) return BadRequest();

            //if the user click on the delete button
            if (dto.ImageDeleted)
            {
                product.MainImage = null;
            } else if (dto.MainImageForm != null)
            {
                product.MainImage = await dto.MainImageForm.ToBytes();
            }

            _mapper.Map(dto, product);
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
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductFormDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);
            product.MainImage = await dto.MainImageForm.ToBytes();
            

            if (ModelState.IsValid)
            {

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
