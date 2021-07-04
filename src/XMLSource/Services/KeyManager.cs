using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace XmlSource.Services
{
    public class KeyManager: IKeyManager
    {
        public string GetKeyFromRequest(HttpRequest request)
        {
            var authHeader = request.Headers.First(h => h.Key == "Authorization").Value.ToString();
            var authHeaderContent = authHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var key = authHeaderContent[1];
            return key;
        }

        public string DecodeKey(string encodedKey)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(encodedKey);
                var decodedKey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                return decodedKey;

            }
            catch
            {
                return "";
            }
        }

    }
}
