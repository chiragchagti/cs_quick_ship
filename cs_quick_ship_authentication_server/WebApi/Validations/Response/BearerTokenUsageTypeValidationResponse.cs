using Domain.Enumeration;

namespace cs_quick_ship_authentication_server.Validation.Response
{
    public class BearerTokenUsageTypeValidationResponse : BaseValidationResponse
    {
        public string Token { get; set; }
        public BearerTokenUsageTypeEnum BearerTokenUsageType { get; set; }
    }
}
