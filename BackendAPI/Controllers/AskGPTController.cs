using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/askGPT")]
    [ApiController]
    public class AskGPTController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetGeneralResponse([FromBody] AskGPTModel userQuery)
        {
            if(userQuery.queryType != "GEN")
            {
                return NotFound("This is not a general user query");
            }
            var response = AskGPTService.GetResponseFromGPT(userQuery.query);
            return Ok(response);
        }
    }
}
