using System.Collections.Generic;

namespace Domain.Common
{
    public class OAuth2ServerHelpers
    {
        public static IList<string> CodeChallenegMethodsSupport = new List<string>()
        {
            Constants.ChallengeMethod.Plain,
            Constants.ChallengeMethod.SHA256
        };


        public static IList<string> OpenIdConnectScopes = new List<string>()
        {
            Constants.OpenIdConnectScopes. OpenId,
            Constants.OpenIdConnectScopes.Profile,
            Constants.OpenIdConnectScopes.Email,
            Constants.OpenIdConnectScopes.Address,
            //Constants.OpenIdConnectScopes.Phone
        };
    }
}
