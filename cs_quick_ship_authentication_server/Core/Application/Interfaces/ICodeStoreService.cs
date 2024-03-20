using Application.Models;

using System.Security.Claims;

namespace Application.Interfaces
{
    public interface ICodeStoreService
    {
        string GenerateAuthorizationCode(AuthorizationCode authorizationCode);
        AuthorizationCode GetClientDataByCode(string key);
        AuthorizationCode UpdatedClientDataByCode(string key, ClaimsPrincipal claimsPrincipal, IList<string> requestdScopes);
        AuthorizationCode RemoveClientDataByCode(string key);
    }
}
