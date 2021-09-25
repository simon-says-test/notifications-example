using Microsoft.AspNetCore.Mvc;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public IActionResult Error() => Problem();
    }
}
