using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using XMLSource.Data;
using XMLSource.Services;

namespace XMLSource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XMLResultsController : ControllerBase
    {
        private XMLDbContext db;
        private readonly IOptions<MyAppSettings> _options;

        public XMLResultsController(IOptions<MyAppSettings> options)
        {
            db = new XMLDbContext();
            _options = options;
        }

        // GET: api/<ResultsController>
        [HttpGet]
        public IActionResult Get()
        {
            var xmlKey = _options.Value.AppKey;
            var request = Request;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result= db.Results.FirstOrDefault();
            var resultToReturn = new ResultToReturn();

            
            if (decodedKey == xmlKey)
            {
                resultToReturn.Error = 0;
                resultToReturn.Temperature = result.Temperature;
                resultToReturn.Pressure = result.Pressure;
                var serializedResult = SerializeObject(resultToReturn);
                return Ok(serializedResult);
            }

            return Unauthorized();
        }

        static string GetKeyFromRequest(HttpRequest request)
        {
            var authHeader = request.Headers.First(h => h.Key == "Authorization").Value.ToString();
            var authHeaderContent = authHeader.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var key = authHeaderContent[1];
            return key;
        }

        static string DecodeKey(string encodedKey)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedKey);
            var decodedKey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return decodedKey;
        }

        public static string SerializeObject( ResultToReturn toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

    }
}
