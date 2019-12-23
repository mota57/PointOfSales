using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Threading.Tasks;
using PointOfSales.Core.Infraestructure;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TablePlugin;

namespace PointOfSales.WebUI.Controllers
{

    [Route("api/[controller]")]
    public class MerchantController  : Controller 
    {
        private readonly POSContext _context;
        private readonly ILogger<MerchantController> _logger;
        private readonly IMapper _mapper;

        public MerchantController(
            ILogger<MerchantController> logger,
            IMapper mapper,
            POSContext context)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._context = context;
        }

        public class ProductView 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Note { get; set; }
            public string CategoryName { get; set; }
            public string MainImage { get; set; }
            public bool IsProductRent { get; set; }
        } 

        [HttpPost("[action]/")]
        public async Task<ActionResult> ProductPosList([FromBody] RequestTableParameter parameter)
        {
            try
            {

                //var productConfig = new CustomQueryConfig("v_product_merchant",
                //        new QueryField(nameof(Product.Id), display:false),
                //        new QueryField(nameof(Product.Name)),
                //        new QueryField(nameof(Product.Price)),
                //        new QueryField(nameof(Product.Note), sort:false),
                //        new QueryField("CategoryName"),
                //        new QueryField(nameof(Product.MainImage), filter:false, sort:false),
                //        new QueryField(nameof(Product.IsProductForRent))
                //  );

                /*
                 */


                var productConfig = DataTableRepository.GetByConfig("v_product_merchant1");
                productConfig.ConnectionString = GlobalVariables.Connection;
                productConfig.Provider = TablePlugin.DatabaseProvider.SQLite;

                var paginator = new CustomQueryWithPagination();
                var products = await paginator.GetAsync<ProductView>(productConfig, parameter);





                string result = JsonConvert.SerializeObject(products, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                });

                return Ok(result);

            } catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }

            //return Ok(products);
            //     id: i + 1,
            //     title: faker.name.firstName(),
            //     imgSrc: imgSrc,
            //     body: faker.lorem.sentence(),
            //     price: faker.random.number({ min: 5, max: 20000 }),
            //     isProductForRent: true,
            //     startDate: '',
            //     endDate: '',
            //     disscountType: 'none',
            //     discountId: -1,

        }

        //[HttpGet("[action]/{category:int?}")]
        //public async Task<ActionResult> ProductPosList([FromRoute] int category)
        //{
        //}

        // POST: api/Categories
        [HttpPost("[action]")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> Pay([FromBody] OrderForPayDTO vm)
        {
            //save to order, orderDetail, paymentOrder
            var order = _mapper.Map<Order>(vm);

            //check that products are for rent
            if (TryValidateModel(order))
            {
                _context.Add(order);
                _context.SaveChanges();
            }

            return BadRequest(ModelState);
        }

    }
}
