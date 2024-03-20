using Microsoft.AspNetCore.Mvc;
using Application.OauthResponse;

namespace cs_quick_ship_authentication_server.Controllers
{
    public class DiscoveryEndpointController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DiscoveryEndpointController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    
        // .well-known/openid-configuration
        [HttpGet("~/.well-known/openid-configuration")]
        public JsonResult GetConfiguration()
        {
            var response = new DiscoveryResponse
            {
                issuer = "https://localhost:56255",
                authorization_endpoint = "https://localhost:56255/Home/Authorize",
                token_endpoint = "https://localhost:56255/Home/Token",
                token_endpoint_auth_methods_supported = new string[] { "client_secret_basic", "private_key_jwt" },
                token_endpoint_auth_signing_alg_values_supported = new string[] { "RS256", "ES256" },

                acr_values_supported = new string[] {"urn:mace:incommon:iap:silver", "urn:mace:incommon:iap:bronze"},
                response_types_supported = new string[] { "code", "code id_token", "id_token", "token id_token" },
                subject_types_supported = new string[] { "public", "pairwise" },
                userinfo_endpoint = "https://localhost:56255/api/UserInfo/GetUserInfo",
                userinfo_encryption_enc_values_supported = new string[] { "A128CBC-HS256", "A128GCM" },
                id_token_signing_alg_values_supported = new string[] { "RS256", "ES256", "HS256" , "SHA256" },
                id_token_encryption_alg_values_supported = new string[] { "RSA1_5", "A128KW" },
                id_token_encryption_enc_values_supported = new string[] { "A128CBC-HS256", "A128GCM" },
                request_object_signing_alg_values_supported = new string[] { "none", "RS256", "ES256" },
                display_values_supported = new string[] { "page", "popup" },
                claim_types_supported = new string[] { "normal", "distributed" },
                //jwks_uri = "https://localhost:56255/jwks.json",
                scopes_supported = new string[] { "openid", "profile", "email", "address", "phone", "offline_access" },
                claims_supported = new string[] { "sub", "iss", "auth_time", "acr", "name", "given_name",
                    "family_name", "nickname", "profile", "picture", "website", "email", "email_verified",
                    "locale", "zoneinfo" },
                claims_parameter_supported = true,
                service_documentation = "https://localhost:56255/connect/service_documentation.html",
                ui_locales_supported = new string[] { "en-US", "en-GB", "en-CA", "fr-FR", "fr-CA" },
                introspection_endpoint = "https://localhost:56255/Introspections/TokenIntrospect"

            };

            return Json(response);
        }

        // jwks.json
        [HttpGet("~/jwks.json")]
        public FileResult Jwks()
        {
            string path =  "wwwroot/jwks.json";//Path.Combine(_hostingEnvironment.WebRootPath, "jwks.json");
            if (!System.IO.File.Exists(path))
                return null;

            var fileBytes = System.IO.File.ReadAllBytes(path);

            // Determine the file content type
            var contentType = "application/json"; // You can set appropriate content type based on file type

            // Return the file
            return File(fileBytes, contentType, Path.GetFileName(path));
        }
    }
}
