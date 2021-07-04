using Microsoft.AspNetCore.Http;

namespace JsonSource.Services
{
    public interface IKeyManager
    {
        string GetKeyFromRequest(HttpRequest request);

        string EncodeKey(string originalKey);

    }
}
