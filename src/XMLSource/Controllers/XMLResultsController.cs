using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public IEnumerable<Result> Get()
        {
            var xmlKey = _options.Value.AppKey;
            var request = Request;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            if (decodedKey == xmlKey)
            {
                return db.Results.ToList();
            }

            return null;
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
    }
}
