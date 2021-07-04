using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonSource.Services
{
    public interface IKeyManager
    {
        string GetKeyFromRequest(HttpRequest request);

        string DecodeKey(string encodedKey);

    }
}
