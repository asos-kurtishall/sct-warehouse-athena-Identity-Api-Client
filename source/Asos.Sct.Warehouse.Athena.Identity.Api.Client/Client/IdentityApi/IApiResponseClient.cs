using System.Threading;
using System.Threading.Tasks;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi;

public interface IApiResponseClient
{
    Task<ApiResponse<T>> GetAsync<T>(string controller, string action, CancellationToken cancellationToken);

    Task<ApiResponse<T>> PostAsync<T>(string controller, string action, object content, CancellationToken cancellationToken);

    Task<ApiResponse> PostAsync(string controller, string action, object content, CancellationToken cancellationToken);

    Task<ApiResponse> PutAsync(string controller, string action, object content, CancellationToken cancellationToken);

    Task<ApiResponse<T>> PutAsync<T>(string controller, string action, object content, CancellationToken cancellationToken);
}
