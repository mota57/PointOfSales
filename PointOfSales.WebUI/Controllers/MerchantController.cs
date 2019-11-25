using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Threading.Tasks;
using System.Linq;


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

                order.StatusOrder = StatusOrder.CLOSE;

                _context.Add(order);
                _context.SaveChanges();
            }

            return BadRequest(ModelState);
        }

    }
}
