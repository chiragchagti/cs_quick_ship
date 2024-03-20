using System.Collections.Generic;

namespace Application.Models
{
    public class Client
    {
        public Client()
        {

        }

        public string ClientName { get; set; }
        public string ClientId { get; set; }

        /// <summary>
        /// Client Password
        /// </summary>
        public string ClientSecret { get; set; }

        public IList<string> GrantTypes { get; set; }

        /// <summary>
        /// by default false
        /// </summary>
        public bool IsActive { get; set; } = false;
        public IList<string> AllowedScopes { get; set; }

        public string ClientUri { get; set; }
        public string RedirectUri { get; set; }

        public bool UsePkce { get; set; }

        /// <summary>
        /// Get or set the name of the clients/protected resource that are releated to this Client.
        /// </summary>
        public IList<string> AllowedProtectedResources { get; set; }
    }
}
