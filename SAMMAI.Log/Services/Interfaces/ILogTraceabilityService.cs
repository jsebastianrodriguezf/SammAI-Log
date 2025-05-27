using SAMMAI.Transverse.Models.Endpoints.Log.LogTraceability;

namespace SAMMAI.Log.Services.Interfaces
{
    public interface ILogTraceabilityService
    {
        Task InsertLogTraceability(InsertLogTraceabilityRequest input);
    }
}
