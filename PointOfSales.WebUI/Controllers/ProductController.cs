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
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PointOfSales.WebUI.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApplicationBaseController<Product>
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

        public class FileContentManager
        {
            private readonly IHostingEnvironment webhost;
            private string folderPath;
            private string rootFolder = "images";

            public FileContentManager(IHostingEnvironment webhost, string folderPath)
            {
                this.webhost = webhost;
                this.folderPath = folderPath;
            }

            public List<OperationResponse> RemoveFileFromDisk(string[] fileNames)
            {
                var operations = new List<OperationResponse>();

                foreach(var filename in fileNames)
                {

                    try
                    {
                        if (System.IO.File.Exists(GetFilePath(filename)))
                        {
                            System.IO.File.Delete(GetFilePath(filename));
                        }

                        operations.Add(new OperationResponse());
                    } catch (IOException ex)
                    {
                        operations.Add(new OperationResponse(ex));
                    }
                }
                return operations;
            }

            private string GetFilePath(string filename)
                => Path.Combine(webhost.WebRootPath, rootFolder, folderPath, filename);


            public async Task<List<string>> SaveFileToDisk(IFormFile[] files)
            {
                var result = new List<string>();

                var uploads = Path.Combine(webhost.WebRootPath, rootFolder, folderPath);
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var extension = Path.GetExtension(file.Name);
                        var fileName = $"{Guid.NewGuid()}.{extension}";
                        var filePath = Path.Combine(uploads, fileName.ToString());
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        result.Add(fileName);
                    }
                }
                return result;
            }
        }



        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductFormDTO dto,
              [FromServices] IHostingEnvironment webHost)
        {
            FileContentManager fileContentManager = new FileContentManager(webHost, "product");

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
                    Thread task = new Thread(() => fileContentManager.RemoveFileFromDisk(new string[] { product.MainImage }));
                    task.Start();
                    product.MainImage = null;
                } else if (dto.MainImageForm != null)
                {
                    //save to disk
                    var fileNames = await fileContentManager.SaveFileToDisk(new IFormFile[] { dto.MainImageForm });
                    dto.MainImage = fileNames[0];
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
