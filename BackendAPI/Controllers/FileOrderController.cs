using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection.Emit;

namespace BackendAPI.Controllers
{
    [Route("fetchFileOrder")]
    [ApiController]
    public class FileOrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetFileOrder(string useCase, int version)
        {
            string? fileOrderJson = FileOrderService.GetFileOrderFromBlob(useCase);

            if(string.IsNullOrEmpty(fileOrderJson))
            {
                return NotFound("Use Case does not exist in Database.");
            }
            try
            {
                var fileOrder = JsonHelper.Deserialize<List<FileOrderModel>>(fileOrderJson);
                return Ok(fileOrder);
            }
            catch (Exception ex)
            {
                return BadRequest("Error in parsing the JSON: " + ex.Message);
            }
        }

    }
}