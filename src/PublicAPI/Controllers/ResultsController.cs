using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PublicAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublicAPI.Controllers
{
    [Route("/today")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IOptions<MyAppSettings> _options;
        private readonly string _jsonAppKey;
        private readonly string _xmlAppKey;
        private string _jsonUrl = "http://localhost:58270/today";
        private string _xmlUrl = "http://localhost:43529/today";



        public ResultsController(IOptions<MyAppSettings> options)
        {
            _client = new HttpClient();
            _options = options;
            _jsonAppKey= _options.Value.JsonAppKey;
            _xmlAppKey= _options.Value.XmlAppKey;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var finalResult = new StringBuilder();
            var jsonEncodedKey = EncodeKey(_jsonAppKey);

            //making request to Source 1 (JsonSource)
            HttpResponseMessage jsonResponse = await _client.SendAsync(CreateRequestMessage(jsonEncodedKey, _jsonUrl));
            if (jsonResponse.IsSuccessStatusCode)
            {
                var jsonResult = await jsonResponse.Content.ReadAsStringAsync();
                finalResult.AppendLine("This is the result from the Source 1 in JSON format");
                finalResult.AppendLine(jsonResult);
                return finalResult.ToString().TrimEnd();
            }

            //making request to Source 2 (XmlSource)
            var xmlEncodedKey = EncodeKey(_xmlAppKey);
            HttpResponseMessage xmlResponse = await _client.SendAsync(CreateRequestMessage(xmlEncodedKey, _xmlUrl));
            if (xmlResponse.IsSuccessStatusCode)
            {
                var xmlResult = await xmlResponse.Content.ReadAsStringAsync();
                finalResult.AppendLine("This is the result from the second source in XML");
                finalResult.AppendLine(xmlResult);
                return finalResult.ToString().TrimEnd();
            }

            //returning error message
            finalResult.AppendLine("Error.");
            finalResult.AppendLine("Unfortunately none of our sources was able to provide data for you.");
            finalResult.AppendLine("You may try again later.");
            return finalResult.ToString().TrimEnd();

        }

        static string EncodeKey(string key)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(key);
            var encodedKey = System.Convert.ToBase64String(plainTextBytes);
            return encodedKey;
        }

        static HttpRequestMessage CreateRequestMessage (string encodedKey, string url)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Authorization", $"Basic {encodedKey}"}
                },

            };
        }

    }
}
