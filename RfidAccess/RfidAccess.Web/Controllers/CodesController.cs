using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.Records;
using RfidAccess.Web.ViewModels;
using RfidAccess.Web.ViewModels.Base;

namespace RfidAccess.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesController(IRecordService recordService) : ControllerBase
    {
        private readonly IRecordService recordService = recordService;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddRecord([FromBody] ValueRequest request)
        {
            try
            {
                //if (Request.Host.Value != "127.0.0.1:8000")
                //{
                //    return Unauthorized();
                //}

                Result result = await recordService.InsertCode(request.Value);
                if (result.IsFailed)
                {
                    if (result.Message == "PERSON_INSERTED")
                        return StatusCode(StatusCodes.Status201Created, "CREATED");
                    return BadRequest(result.Message);
                }

                return Ok("ALLOWED");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("Healthy");
        }
    }
}
