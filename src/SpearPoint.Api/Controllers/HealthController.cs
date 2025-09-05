using Microsoft.AspNetCore.Mvc;

namespace SpearPoint.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new
        {
            status = "ok",
            service = "SpearPoint.Api",
            utc = DateTime.UtcNow
        });
    }
}
