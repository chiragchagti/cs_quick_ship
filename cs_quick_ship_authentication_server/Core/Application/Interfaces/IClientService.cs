/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Text;
using Application.Models;
using Application.Models.Context;
using Domain.Common;
using Domain.Configuration;
using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces
{
    public interface IClientService
    {
        CheckClientResult VerifyClientById(string clientId, bool checkWithSecret = false, string clientSecret = null,
            string grantType = null);

        bool SearchForClientBySecret(string grantType);
        AudienceValidator ValidateAudienceHandler(IEnumerable<string> audiences, SecurityToken securityToken,
            TokenValidationParameters validationParameters, Client client, string token);

        Task<CheckClientResult> GetClientByUriAsync(string clientUrl);

        Task<CheckClientResult> GetClientByIdAsync(string clientId);
    }

    public class ClientService : IClientService
    {
        private readonly ClientStore _clientStore = new ClientStore();
        private readonly OAuthServerOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseDBContext _dbContext;
        public ClientService(IHttpContextAccessor httpContextAccessor,
            BaseDBContext context
            , IOptions<OAuthServerOptions> options
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = context;
            _options = options.Value;
        }

        public Task<CheckClientResult> GetClientByIdAsync(string clientId)
        {
            Client client = new();
            if (_options.Provider == "DB")
            {
                var dbClient = _dbContext.OAuthApplications.Where(x => x.ClientId == clientId).FirstOrDefault();
                if (dbClient != null)
                {

                client.ClientUri = dbClient.ClientUri;
                client.ClientId = clientId;
                client.ClientSecret = dbClient.ClientSecret;
                client.ClientName = dbClient.ClientName;
                client.RedirectUri = dbClient.RedirectUris;
                client.IsActive = dbClient.IsActive;
                client.AllowedScopes =  dbClient.AllowedScopes.Split(' ');
                client.GrantTypes = dbClient.GrantTypes.Split(' ');
                }
            }
            else
            {
                client = _clientStore.Clients.Where(x => x.ClientId == clientId).FirstOrDefault();
            }
            var response = new CheckClientResult
            {
                Client = client,
                IsSuccess = true
            };
            return Task.FromResult(response);
            //var client = await _dbContext.OAuthApplications.FirstOrDefaultAsync
            //    (x=>x.ClientId ==  clientId);
            //if(client == null)
            //{

            //}
            //else
            //{
            //   var c = _clientStore.Clients.Where(x => x.ClientId == clientId).FirstOrDefault();
            //}
        }


        public Task<CheckClientResult> GetClientByUriAsync(string clientUrl)
        {
            Client client = new();
            if (_options.Provider == "DB")
            {
                var dbClient = _dbContext.OAuthApplications.Where(x => x.ClientUri == clientUrl).FirstOrDefault();
                if (dbClient != null)
                {

                client.ClientUri = dbClient.ClientUri;
                client.ClientId = dbClient.ClientId;
                client.ClientSecret = dbClient.ClientSecret;
                client.ClientName = dbClient.ClientName;
                client.RedirectUri = dbClient.RedirectUris;
                client.IsActive = dbClient.IsActive;
                client.AllowedScopes = dbClient.AllowedScopes.Split(' ');
                client.GrantTypes = dbClient.GrantTypes.Split(' ');
                }
            }
            else
            {
                client = _clientStore.Clients.Where(x => x.ClientUri == clientUrl).FirstOrDefault();
            }
            var response = new CheckClientResult
            {
                Client = client,
                IsSuccess = true
            };
            return Task.FromResult(response);
        }

        public CheckClientResult VerifyClientById(string clientId, bool checkWithSecret = false, string clientSecret = null,
            string grantType = null)
        {
            CheckClientResult result = new CheckClientResult() { IsSuccess = false };

            if (!string.IsNullOrWhiteSpace(grantType) &&
                grantType == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription())
            {
                var data = _httpContextAccessor.HttpContext;
                var authHeader = data.Request.Headers["Authorization"].ToString();
                if (authHeader == null)
                    return result;
                if (!authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
                    return result;

                var parameters = authHeader.Substring("Basic ".Length);
                var authorizationKeys = Encoding.UTF8.GetString(Convert.FromBase64String(parameters));

                var authorizationResult = authorizationKeys.IndexOf(':');
                if (authorizationResult == -1)
                    return result;
                clientId = authorizationKeys.Substring(0, authorizationResult);
                clientSecret = authorizationKeys.Substring(authorizationResult + 1);

            }



            if (!string.IsNullOrWhiteSpace(clientId))
            {
                Client client = new();
                if (_options.Provider == "DB")
                {
                    var dbClient = _dbContext.OAuthApplications.Where(x =>
                    x.ClientId == clientId)
                    .FirstOrDefault();
                    if (dbClient != null)
                    {
                        client.ClientUri = dbClient.ClientUri;
                        client.ClientId = clientId;
                        client.ClientSecret = dbClient.ClientSecret;
                        client.ClientName = dbClient.ClientName;
                        client.RedirectUri = dbClient.RedirectUris;
                        client.IsActive = dbClient.IsActive;
                        client.AllowedScopes = dbClient.AllowedScopes.Split(' ');
                        client.GrantTypes = dbClient.GrantTypes.Split(' ');
                    }
                }
                else
                {
                    client = _clientStore
                    .Clients
                    .Where(x =>
                    x.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                }

                if (client != null)
                {
                    if (checkWithSecret && !string.IsNullOrEmpty(clientSecret))
                    {
                        bool hasSamesecretId = client.ClientSecret.Equals(clientSecret, StringComparison.InvariantCulture);
                        if (!hasSamesecretId)
                        {
                            result.Error = ErrorTypeEnum.InvalidClient.GetEnumDescription();
                            return result;
                        }
                    }
                    // check if client is enabled or not

                    if (client.IsActive)
                    {
                        result.IsSuccess = true;
                        result.Client = client;

                        return result;
                    }
                    else
                    {
                        result.ErrorDescription = ErrorTypeEnum.UnAuthoriazedClient.GetEnumDescription();
                        return result;
                    }
                }
            }

            result.ErrorDescription = ErrorTypeEnum.AccessDenied.GetEnumDescription();
            return result;
        }


        public bool SearchForClientBySecret(string grantType)
        {
            if (grantType == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription() ||
                grantType == AuthorizationGrantTypesEnum.RefreshToken.GetEnumDescription() ||
                grantType == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription())
                return true;

            return false;
        }

        public AudienceValidator ValidateAudienceHandler(IEnumerable<string> audiences, SecurityToken securityToken,
            TokenValidationParameters validationParameters, Client client, string token)
        {
            Func<IEnumerable<string>, SecurityToken, TokenValidationParameters, bool> handler = (audiences, securityToken, validationParameters) =>
            {
                // Check the Token the Back Store.
                var tokenInDb = _dbContext.OAuthTokens.FirstOrDefault(x => x.Token == token);
                if (tokenInDb == null)
                    return false;

                if (tokenInDb.Revoked)
                    return false;

                return true;
            };
            return new AudienceValidator(handler);
        }
    }
}
