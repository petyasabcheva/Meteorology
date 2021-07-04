using Microsoft.AspNetCore.Http;

namespace XmlSource.Services
{
    public interface IKeyManager
    {
        string GetKeyFromRequest(HttpRequest request);
        string EncodeKey(string originalKey);
    }
}
