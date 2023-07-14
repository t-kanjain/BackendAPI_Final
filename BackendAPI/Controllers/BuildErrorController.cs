using System.Reflection.Metadata.Ecma335;
using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/buildError")]
    [ApiController]
    public class BuildErrorController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetPotentialFix([FromBody] string buildOutput)
        {
            var potentialFix = BuildErrorService.GetFixFromBuild(buildOutput);
            return Ok(potentialFix);
        }

        [HttpPost("fix")]
        public IActionResult GetModifiedCode([FromBody] BuildErrorModel errorFile)
        {
            var errorList = new List<string>();
            foreach (var error in errorFile.ErrorList)
            {
                errorList.Add($"{error.ErrorMessage} encountered at Line: {error.LineNumber} with a potential fix: {error.PotentialFix}");
            }

            var modifiedCode = BuildErrorService.FixBuildError(errorFile.inputFile, string.Join("\n", errorList));
            return Ok(modifiedCode);
        }
    }
}
