using Microsoft.AspNetCore.Mvc;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.ActionFilters;
using SAMMAI.Transverse.Constants;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Endpoints.Log.LogTraceability;
using SAMMAI.Transverse.Models.Response.BaseApi;
using static SAMMAI.Log.Utility.Constants.ApiRoutesConstants;

namespace SAMMAI.Log.Controllers
{
    [Route($"{BaseApi}/log-traceability")]
    [ApiController]
    public class LogTraceabilityController : ControllerBase
    {
        private readonly ILogger<LogTraceabilityController> _logger;
        private readonly ILogTraceabilityService _logTraceabilityService;

        public LogTraceabilityController(
            ILogger<LogTraceabilityController> logger,
            ILogTraceabilityService logTraceabilityService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logTraceabilityService = logTraceabilityService ?? throw new ArgumentNullException(nameof(logTraceabilityService));
        }

        /// <summary>
        /// Insert a log traceability
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Create a log traceability
        /// 
        /// Authorization: Public/Private
        /// 
        ///     Headers:
        ///         Authorization: <c>Bearer {Token}</c>
        /// </remarks>
        /// <response code="201">Success</response>
        [PublicIdentifierFilter]
        [HttpPost]
        [Route("insert")]
        [Produces(GeneralConstants.ContentType.Json)]
        [ProducesResponseType(typeof(BaseSuccessApiResponse), (int)StatusCodeEnum.CREATED)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.BAD_REQUEST)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.INTERNAL_SERVER_ERROR)]
        public async Task<ActionResult> InsertRequest([FromBody] InsertLogTraceabilityRequest request)
        {
            await _logTraceabilityService.InsertLogTraceability(request);

            return StatusCode((int)StatusCodeEnum.CREATED, ResponseHelper.SetResponse(StatusCodeEnum.CREATED));
        }
    }
}
