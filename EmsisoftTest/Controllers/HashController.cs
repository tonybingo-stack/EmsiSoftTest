using Microsoft.AspNetCore.Mvc;

namespace EmsisoftTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashController : ControllerBase
    {

        [HttpGet(Name = "GetHashes")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
        [HttpPost(Name = "GenerateHashes")]
        public IActionResult Post()
        {
            return Ok("ok");
        }
    }
}