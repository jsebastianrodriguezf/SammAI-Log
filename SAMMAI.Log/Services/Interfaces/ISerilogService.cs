using SAMMAI.Log.Models.Request;

namespace SAMMAI.Log.Services.Interfaces
{
    public interface ISerilogService
    {
        Task Insert(List<SerilogRequest> input);
    }
}
