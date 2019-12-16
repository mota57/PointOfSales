using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Threading.Tasks;
using System.Linq;
using PointOfSales.Core.Extensions;

namespace PointOfSales.WebUI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : Controller
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

        [HttpGet("[action]/{category:string?}")]
        public async Task<ActionResult> ProductPosList([FromRoute] string category)
        {
            var products = _context.Product
                 .Where(p => p.Category.Name.EqualIgnoreCase(category))
                 .Select(p =>
                 new
                 {

                     p.Id,
                     p.Name,
                     p.Note,
                     p.Price,
                     p.IsProductForRent,
                     StartDate = "",
                     EndDate = "",
                     p.DiscountType,
                     p.DiscountId
                 });

            return Ok(products);
            
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

        [HttpGet("[action]/{category:int?}")]
        public async Task<ActionResult> ProductPosList([FromRoute] int category)
        {

        }

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
