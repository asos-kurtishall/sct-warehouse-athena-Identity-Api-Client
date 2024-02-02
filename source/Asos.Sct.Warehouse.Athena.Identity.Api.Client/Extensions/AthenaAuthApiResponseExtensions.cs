using System.Collections.Generic;
using System.Linq;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi;

public static class AthenaAuthApiResponseExtensions
{
    public static string GetValidationErrorMessageString(this ApiResponse apiResponse)
    {
        var validationErrors = apiResponse?.Error?.ValidationErrors ?? new List<ValidationError>();

        if (!validationErrors.Any()) return string.Empty;

        var validationErrorMessages = validationErrors.Select(e => e.Message).ToList();
        return string.Join("\n", validationErrorMessages);
    }
}
