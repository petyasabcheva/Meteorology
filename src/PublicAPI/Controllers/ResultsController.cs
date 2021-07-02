using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PublicAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IOptions<MyAppSettings> _options;
        private readonly string _jsonAppKey;
        private readonly string _xmlAppKey;
        private string _jsonUrl = "http://localhost:58270/api/jsonresults/1";
        private string _xmlUrl = "http://localhost:43529/api/xmlresults/1";



        public ResultsController(IOptions<MyAppSettings> options)
        {
            _client = new HttpClient();
            _options = options;
            _jsonAppKey= _options.Value.JsonAppKey;
            _xmlAppKey= _options.Value.XMLAppKey;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var finalResult = new StringBuilder();
            finalResult.AppendLine("This is the result from the first source in JSON");
            var jsonResult = "";
            var jsonEncodedKey = EncodeKey(_jsonAppKey);
            
            HttpResponseMessage jsonResponse = await _client.SendAsync(CreateRequestMessage(jsonEncodedKey, _jsonUrl));

            if (jsonResponse.IsSuccessStatusCode)
            {
                jsonResult = await jsonResponse.Content.ReadAsStringAsync();
            }
            finalResult.AppendLine(jsonResult);



            finalResult.AppendLine("This is the result from the second source in XML");
            var xmlResult = "";
            var xmlEncodedKey = EncodeKey(_xmlAppKey);

            HttpResponseMessage xmlResponse = await _client.SendAsync(CreateRequestMessage(xmlEncodedKey,_xmlUrl));
            if (xmlResponse.IsSuccessStatusCode)
            {
                xmlResult = await xmlResponse.Content.ReadAsStringAsync();
            }

            finalResult.AppendLine(xmlResult);
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
                Headers = { { "Authorization", $"Basic {encodedKey}" } },
            };
        }

    }
}
