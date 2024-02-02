using Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public bool IsFailure { get; set; }

        public Error Error { get; set; }
    }
}
