using System.ComponentModel;

namespace Application.Models
{
    public enum AuthorizationGrantTypesEnum : byte
    {
        [Description("code")] Code,

        [Description("client_credentials")] ClientCredentials,

        [Description("refresh_token")] RefreshToken,

        [Description("authorization_code")] AuthorizationCode
    }
}
