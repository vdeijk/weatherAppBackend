using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeatherApi.Controllers
{
    /// <summary>
    /// Handles global errors and returns appropriate responses.
    /// </summary>
    [ApiController]
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IHostEnvironment _env;

        public ErrorController(ILogger<ErrorController> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Handles unhandled exceptions and returns error details.
        /// </summary>
        /// <returns>Problem details response.</returns>
        [HttpGet]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            _logger.LogError(exception, "Unhandled exception occurred.");

            if (_env.IsDevelopment())
            {
                return Problem(
                    detail: exception?.ToString(),
                    title: "Unhandled Exception",
                    statusCode: 500
                );
            }
            else
            {
                return Problem(
                    detail: "An unexpected error occurred.",
                    title: "Error",
                    statusCode: 500
                );
            }
        }
    }
}
