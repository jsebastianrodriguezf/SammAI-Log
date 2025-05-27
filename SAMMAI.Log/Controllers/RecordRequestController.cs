using Microsoft.AspNetCore.Mvc;
using SAMMAI.Log.Services.Interfaces;
using SAMMAI.Log.Utility.ActionFilters;
using SAMMAI.Transverse.Constants;
using SAMMAI.Transverse.Helpers;
using SAMMAI.Transverse.Models.Endpoints.Log.RecordRequest;
using SAMMAI.Transverse.Models.Response.BaseApi;
using static SAMMAI.Log.Utility.Constants.ApiRoutesConstants;

namespace SAMMAI.Log.Controllers
{
    [Route($"{BaseApi}/record-request")]
    [ApiController]
    public class RecordRequestController : ControllerBase
    {
        private readonly ILogger<RecordRequestController> _logger;
        private readonly IRecordRequestService _recordRequestService;

        public RecordRequestController(
            ILogger<RecordRequestController> logger,
            IRecordRequestService recordRequestService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recordRequestService = recordRequestService ?? throw new ArgumentNullException(nameof(recordRequestService));
        }

        /// <summary>
        /// Log a request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Create a log for a request
        /// 
        /// Authorization: Public/Private
        /// 
        ///     Headers:
        ///         Authorization: <c>Bearer {Token}</c>
        /// </remarks>
        /// <response code="201">Success</response>
        [PublicIdentifierFilter]
        [HttpPost]
        [Route("insert/request")]
        [Produces(GeneralConstants.ContentType.Json)]
        [ProducesResponseType(typeof(BaseSuccessApiResponseWithData<InsertRequestResponse>), (int)StatusCodeEnum.OK)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.BAD_REQUEST)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.INTERNAL_SERVER_ERROR)]
        public async Task<ActionResult<Object>> InsertRequest([FromBody] InsertRequestRequest request)
        {
            InsertRequestResponse response = await _recordRequestService.InsertRequest(request);

            return StatusCode((int)StatusCodeEnum.CREATED, ResponseHelper.SetResponseWithData(StatusCodeEnum.CREATED, response));
        }

        /// <summary>
        /// Log a response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Create a log for a response
        /// 
        /// Authorization: Public/Private
        /// 
        ///     Headers:
        ///         Authorization: <c>Bearer {Token}</c>
        /// </remarks>
        /// <response code="204">No Content</response>
        [PublicIdentifierFilter]
        [HttpPost]
        [Route("insert/response")]
        [Produces(GeneralConstants.ContentType.Json)]
        [ProducesResponseType((int)StatusCodeEnum.NO_CONTENT)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.BAD_REQUEST)]
        [ProducesResponseType(typeof(BaseBadRequestApiResponse), (int)StatusCodeEnum.INTERNAL_SERVER_ERROR)]
        public async Task<ActionResult<Object>> InsertResponse([FromBody] InsertResponseRequest request)
        {
            await _recordRequestService.InsertResponse(request);

            return NoContent();
        }
    }
}
