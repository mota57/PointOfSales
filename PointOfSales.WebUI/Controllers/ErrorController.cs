using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace PointOfSales.WebUI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public ErrorController(ILogger<ErrorController> logger)
        {
            Logger = logger;
        }

        public ILogger<ErrorController> Logger { get; }

        [Route("/error")]
        public ActionResult Error(
            [FromServices] IHostingEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            var isDev = webHostEnvironment.IsDevelopment();
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = feature?.Path,
                Title = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "An error occurred.",
                Detail = isDev ? $"{ex.StackTrace}\n\nInner Exception " +
                $"Message{ex.InnerException?.Message}\n\n{ex.InnerException?.StackTrace}" : null,

            };
            Logger.LogInformation("====ERROR CONTROLLER INFORMATION ======");
            Logger.LogError(problemDetails.Title);
            Logger.LogError(problemDetails.Detail);
            Logger.LogInformation("====ERROR CONTROLLER INFORMATION ======");


            return StatusCode(problemDetails.Status.Value, problemDetails);
        }
    }
}
