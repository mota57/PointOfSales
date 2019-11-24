using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Threading.Tasks;


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
            //save to order
            //save to orderdetail
            var order = _mapper.Map<Order>(vm);
            
            //check that products are for rent
            if(TryValidateModel(order))
            {
                _context.Add(order);
                _context.SaveChanges();
            }
            
        
            _logger.LogInformation("thread suspend for 2 seconds");
            System.Threading.Thread.Sleep(2000);
            _logger.LogInformation("thread continue..");
        
            return BadRequest(ModelState);
        }
    }
}
