using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonSource.Services
{
    public class KeyManager :IKeyManager
    {
        public string GetKeyFromRequest(HttpRequest request)
        {
            var authHeader = request.Headers.First(h => h.Key == "Authorization").Value.ToString();
            var authHeaderContent = authHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var key = authHeaderContent[1];
            return key;
        }

        public string EncodeKey(string originalKey)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(originalKey);
            var encodedKey = System.Convert.ToBase64String(plainTextBytes);
            return encodedKey;
        }
    }
}
