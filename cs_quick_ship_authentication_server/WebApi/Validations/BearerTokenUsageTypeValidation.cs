using cs_quick_ship_authentication_server.Validation.Response;
using Domain;
using Domain.Enumeration;

namespace cs_quick_ship_authentication_server.Validation
{
    public class BearerTokenUsageTypeValidation : IBearerTokenUsageTypeValidation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BearerTokenUsageTypeValidation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<BearerTokenUsageTypeValidationResponse> ValidateAsync()
        {
            var response = new BearerTokenUsageTypeValidationResponse { Succeeded = false };
            //("Authorization", out var token)
            var authroizationHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization;

            var header = authroizationHeader.First().Trim();
            if (header is not null && header.StartsWith(Constants.AuthenticatedRequestScheme.AuthorizationRequestHeader))
            {
                var token = header.Substring(Constants.AuthenticatedRequestScheme.AuthorizationRequestHeader.Length).Trim();
                if (token is not null && token.Length > 0)
                {
                    response.Succeeded = true;
                    response.Token = token;
                    response.BearerTokenUsageType = BearerTokenUsageTypeEnum.AuthorizationRequestHeader;
                    return Task.FromResult(response);
                }
            }

            var postForm = _httpContextAccessor.HttpContext.Request.Form;
            if(postForm != null && postForm.Any())
            {
                if (postForm.ContainsKey(Constants.AuthenticatedRequestScheme.FormEncodedBodyParameter))
                {
                    var token = postForm.Where(x => x.Key == Constants.AuthenticatedRequestScheme.AuthorizationRequestHeader)
                        .Select(x => x.Value).FirstOrDefault();
                    if(token.Count == 1)
                    {
                        string value = token;
                        if (value is not null)
                        {
                            response.Succeeded = true;
                            response.Token = value;
                            response.BearerTokenUsageType = BearerTokenUsageTypeEnum.FormEncodedBodyParameter;
                            return Task.FromResult(response);
                        }
                    }
                }
            }


            return Task.FromResult(response);
        }
    }
}
