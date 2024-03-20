using Application.OauthRequest;
using Application.OauthResponse;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public interface IAuthorizeResultService
    {
        AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, AuthorizationRequest authorizationRequest);
        TokenResponse GenerateToken(TokenRequest tokenRequest);
    }
}
