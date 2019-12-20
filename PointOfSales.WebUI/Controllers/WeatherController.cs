using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointOfSales.WebUI.Providers;

namespace PointOfSales.WebUI.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IWeatherProvider _weatherProvider;

        public WeatherController(IWeatherProvider weatherProvider)
        {
            _weatherProvider = weatherProvider;
        }

        [HttpGet("[action]")]
        public IActionResult Forecasts([FromQuery(Name = "from")] int from = 0, [FromQuery(Name = "to")] int to = 4)
        {
            //System.Threading.Thread.Sleep(500); // Fake latency
            var quantity = to - from;

            // We should also avoid going too far in the list.
            if (quantity <= 0)
            {
                return BadRequest("You cannot have the 'to' parameter higher than 'from' parameter.");
            }

            if (from < 0)
            {
                return BadRequest("You cannot go in the negative with the 'from' parameter");
            }

            var allForecasts = _weatherProvider.GetForecasts();
            var result = new
            {
                Total = allForecasts.Count,
                Forecasts = allForecasts.Skip(from).Take(quantity).ToArray()
            };

            return Ok(result);
        }

        [NonAction]
        public string getJSObj()
        {
            var obj = new
            {
                username = "andrey",
                callback = new JRaw("function(self) { return function() {self.doSomething()} (this) }")
            };
            // and then serialize using the JsonConvert class
            var jsonObj = JsonConvert.SerializeObject(obj);
            return jsonObj;
        }

        [HttpGet("[action]")]
        public IActionResult Get1()
        {
            return Ok(new JRaw(getJSObj()));
        }

        [HttpGet("[action]")]
        public object Get2()
        {
            return new JRaw(getJSObj());
        }

        [HttpGet("[action]")]
        public IActionResult Get3()
        {
            return Content(getJSObj(), "text/plain");
        }



    }
}
