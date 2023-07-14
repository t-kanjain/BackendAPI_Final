using System.Reflection.Metadata.Ecma335;
using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("buildError")]
    [ApiController]
    public class BuildErrorController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetProcessedBuilOutput([FromBody] string buildOutput)
        {
            var processedErrors = BuildErrorService.ProcessBuildOutput(buildOutput);
            return Ok(processedErrors);
        }

        [HttpPost("solve")]
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

        [HttpPost("potentialFix")]
        public IActionResult GetPotentialFix(string error)
        {

            var potentialFix = BuildErrorService.GetPotentialFix(error);
            return Ok(potentialFix);
        }
    }
}
