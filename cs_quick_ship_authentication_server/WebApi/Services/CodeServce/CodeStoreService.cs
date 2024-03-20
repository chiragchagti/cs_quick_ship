using Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Interfaces;
using Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Application.Models.Context;
using Microsoft.Extensions.Options;
using cs_quick_ship_authentication_server.Services.Configuration;

namespace cs_quick_ship_authentication_server.Services.CodeServce
{
    public class CodeStoreService : ICodeStoreService
    {
        //private readonly ConcurrentDictionary<string, AuthorizationCode> _codeIssued = new ConcurrentDictionary<string, AuthorizationCode>();
        private readonly ConcurrentDictionaryService _concurrentDictionaryService;
        private readonly ClientStore _clientStore = new ClientStore();
        private readonly OAuthServerOptions _options;
        private readonly BaseDBContext _dbContext;

        public CodeStoreService(IOptions<OAuthServerOptions> options, BaseDBContext dbContext, ConcurrentDictionaryService concurrentDictionaryService)
        {
            _options = options.Value;
            _dbContext = dbContext;
            _concurrentDictionaryService = concurrentDictionaryService;
        }



        // Here I genrate the code for authorization, and I will store it 
        // in the Concurrent Dictionary

        public string GenerateAuthorizationCode(AuthorizationCode authorizationCode)
        {
            Client client = new();
            if (_options.Provider == "DB")
            {

                var dbClient = _dbContext.OAuthApplications.Where(x => x.ClientId == authorizationCode.ClientId).SingleOrDefault();
                client.ClientUri = dbClient.ClientUri;
                client.ClientId = dbClient.ClientId;
                client.ClientSecret = dbClient.ClientSecret;
                client.ClientName = dbClient.ClientName;
                client.RedirectUri = dbClient.RedirectUris;
                client.IsActive = dbClient.IsActive;
                client.AllowedScopes = dbClient.AllowedScopes.Split(' ');
                client.GrantTypes = dbClient.GrantTypes.Split(' ');              
            }
            else
            {
                client = _clientStore.Clients.Where(x => x.ClientId == authorizationCode.ClientId).SingleOrDefault();
            }

            if (client != null)
            {
                var rand = RandomNumberGenerator.Create();
                byte[] bytes = new byte[32];
                rand.GetBytes(bytes);
                var code = Base64UrlEncoder.Encode(bytes);

                _concurrentDictionaryService.GetDictionary()[code] = authorizationCode;

                return code;
            }
            return null;
        }

        /// <summary>
        /// Get client data from concurrent dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AuthorizationCode GetClientDataByCode(string key)
        {
            AuthorizationCode authorizationCode;
            if (_concurrentDictionaryService.GetDictionary().TryGetValue(key, out authorizationCode))
            {
                return authorizationCode;
            }
            return null;
        }

        // TODO
        // Before updated the Concurrent Dictionary I have to Process User Sign In,
        // and check the user credienail first
        // But here I merge this process here inside update Concurrent Dictionary method
        public AuthorizationCode UpdatedClientDataByCode(string key, ClaimsPrincipal claimsPrincipal, IList<string> requestdScopes)
        {
            var oldValue = GetClientDataByCode(key);

            if (oldValue != null)
            {
                // check the requested scopes with the one that are stored in the Client Store 
                Client client = new();
                if (_options.Provider == "DB")
                {
                    var dbClient = _dbContext.OAuthApplications.Where(x => x.ClientId == oldValue.ClientId).FirstOrDefault();
                    client.ClientUri = dbClient.ClientUri;
                    client.ClientId = dbClient.ClientId;
                    client.ClientSecret = dbClient.ClientSecret;
                    client.ClientName = dbClient.ClientName;
                    client.RedirectUri = dbClient.RedirectUris;
                    client.IsActive = dbClient.IsActive;
                    client.AllowedScopes = dbClient.AllowedScopes.Split(' ');
                    client.GrantTypes = dbClient.GrantTypes.Split(' ');
                }
                else
                {
                    client = _clientStore.Clients.Where(x => x.ClientId == oldValue.ClientId).FirstOrDefault();
                }

                if (client != null)
                {
                    var clientScope = (from m in client.AllowedScopes
                                       where requestdScopes.Contains(m)
                                       select m).ToList();

                    if (!clientScope.Any())
                        return null;

                    AuthorizationCode newValue = new AuthorizationCode
                    {
                        ClientId = oldValue.ClientId,
                        CreationTime = oldValue.CreationTime,
                        IsOpenId = requestdScopes.Contains("openId") || requestdScopes.Contains("profile"),
                        RedirectUri = oldValue.RedirectUri,
                        RequestedScopes = requestdScopes,
                        Nonce = oldValue.Nonce,
                        CodeChallenge = oldValue.CodeChallenge,
                        CodeChallengeMethod = oldValue.CodeChallengeMethod,
                        Subject = claimsPrincipal,
                    };
                    var result = _concurrentDictionaryService.GetDictionary().TryUpdate(key, newValue, oldValue);

                    if (result)
                        return newValue;
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Delete from concurrent dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AuthorizationCode RemoveClientDataByCode(string key)
        {
            AuthorizationCode authorizationCode;
            var isRemoved = _concurrentDictionaryService.GetDictionary().TryRemove(key, out authorizationCode);
            if (isRemoved)
                return authorizationCode;
            return null;
        }
    }
}
