using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Controllers
{
    public class OrderViewModel //: IValidatableObject
    {
        public OrderViewModel()
        {
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //validate date




        //    return null;
        //}
    }

    
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : Controller
    {
        private readonly POSContext _context;

        public MerchantController(
            ILogger<MerchantController> logger,
            POSContext context)
        {
            this._context = context;
        }



        // POST: api/Categories
        [HttpPost("[action]")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> Pay([FromBody] OrderViewModel vm)
        {
            //save to order
            //save to orderdetail

            if (TryValidateModel(vm))
            {

            }


            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
