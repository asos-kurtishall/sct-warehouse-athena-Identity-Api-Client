using System.Collections.Generic;
using System.Linq;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi
{
    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsValidationError
            => ValidationErrors.Any();

        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
}
