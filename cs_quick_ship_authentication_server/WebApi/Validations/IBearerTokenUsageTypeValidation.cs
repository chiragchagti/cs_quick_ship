using cs_quick_ship_authentication_server.Validation.Response;
using System.Threading.Tasks;

namespace cs_quick_ship_authentication_server.Validation;

public interface IBearerTokenUsageTypeValidation
{
    Task<BearerTokenUsageTypeValidationResponse> ValidateAsync();
}
