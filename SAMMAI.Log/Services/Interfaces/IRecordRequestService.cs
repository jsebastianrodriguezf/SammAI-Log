using SAMMAI.Transverse.Models.Endpoints.Log.RecordRequest;

namespace SAMMAI.Log.Services.Interfaces
{
    public interface IRecordRequestService
    {
        Task<InsertRequestResponse> InsertRequest(InsertRequestRequest input);
        Task InsertResponse(InsertResponseRequest input);
    }
}
