using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("updateCode")]
    
    [ApiController]
    public class UpdateCodeController : ControllerBase
    {
        [HttpPost]
        public IActionResult UpdateCode([FromBody] OldCodeModel oldCode)
        {
            var DSL = UpdateCodeService.GetFilePromptFromBlob(oldCode.useCase, oldCode.FileName);
            if(DSL  == null)
            {
                return NotFound("Use Case or File Name not found in Database");
            }
            var modifiedContent = UpdateCodeService.GetModifiedCode(oldCode.OldContent, DSL);
            var updatedCode = new UpdateCodeModel();
            updatedCode.ModifiedContent = modifiedContent;
            updatedCode.FileName = oldCode.FileName;

            return Ok(updatedCode);
        }
    }
}