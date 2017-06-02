using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CkoShoppingList.Service.Controllers
{
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public ErrorController(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = loggerFactory?.CreateLogger(nameof(ErrorController)) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        [Route("")]
        public IActionResult Index()
        {
            var exception = _httpContextAccessor.HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                _logger.LogError(1, exception.Error, "Unhandled exception occurred.");
            }

            return Ok();
        }
    }
}
