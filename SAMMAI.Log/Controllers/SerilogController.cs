using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMMAI.Log.Models.Request;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Transverse.Constants;
using SAMMAI.Transverse.Models.Response.BaseApi;
using static SAMMAI.Log.Utility.Constants.ApiRoutesConstants;

namespace SAMMAI.Log.Controllers
{
    [ApiController]
    [Route($"{BaseApi}/serilog")]
    public class SerilogController : ControllerBase
    {
        private readon ly ILogger<SerilogController> _logger;
        private readonly ISerilogService _serilogService;

        public SerilogController(
            ILogger<SerilogController> logger,
            ISerilogService serilogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serilogService = serilogService ?? throw new ArgumentNullException(nameof(serilogService));
        }

        /// <summary>
        /// Log a serilog logging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Logs the Serilog logging
        /// 
        /// No Authorization
        /// </remarks>
        /// <response code="204">No Content</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("insert")]
        [Produces(GeneralConstants.ContentType.Json)]
        [ProducesResponseType((int)StatusCodeEnum.NO_CONTENT)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.INTERNAL_SERVER_ERROR)]
        public async Task<ActionResult<Object>> Insert([FromBody] List<SerilogRequest> request)
        {
            _ = Task.Run(async () =>
            {
                await _serilogService.Insert(request);
            });

            return NoContent();
        }
    }
}
