using System.Threading;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IApiClientService
    {
        Task<T> GetAsync<T>(string endpointUri, bool silentWhenFail = false, CancellationToken cancellationToken = default);
        Task<T> PostAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default);
        Task<T> PatchAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default);
        Task<T> PutAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync<T>(string endpointUri, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default);
        void LogClientAppEvent(string message, string heading, bool silent = false);
        Task<T> PutAsync<T>(string endpointUri, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default);
    }
}
