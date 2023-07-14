using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("classifyQuery")]
    [ApiController]
    public class ClassifyQueryController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetQueryClassified([FromBody] string query)
        {
            return Ok();
        }
    }
}
