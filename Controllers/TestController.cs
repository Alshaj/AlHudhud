using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlHudhud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("TestController is working!");
        }
    }
}
