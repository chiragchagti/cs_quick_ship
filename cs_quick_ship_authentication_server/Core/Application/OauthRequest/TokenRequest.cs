using System.Collections.Generic;

namespace Application.OauthRequest
{
    public class TokenRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public string code_verifier { get; set; }
        public IList<string> scope { get; set; }
    }
}
