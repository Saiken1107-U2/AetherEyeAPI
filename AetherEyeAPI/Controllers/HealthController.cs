using Microsoft.AspNetCore.Mvc;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { status = "API está funcionando", timestamp = DateTime.Now });
        }

        [HttpGet("admin-test")]
        public IActionResult AdminTest()
        {
            return Ok(new { 
                message = "Endpoint de administración funcionando",
                timestamp = DateTime.Now,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }
    }
}
