using Application.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_quick_ship_authentication_server.Services.Configuration
{
    public class ConcurrentDictionaryService
    {
        private readonly ConcurrentDictionary<string, AuthorizationCode> _codeIssued = new ConcurrentDictionary<string, AuthorizationCode>();
        
        public ConcurrentDictionary<string, AuthorizationCode> GetDictionary() { return _codeIssued; }
    }
}
