using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSource.Data;
using JSONSource.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JSONSource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonResultsController : ControllerBase
    {
        private JsonDbContext db;
        private readonly IOptions<MyAppSettings> _options;


        public JsonResultsController(IOptions<MyAppSettings> options)
        {
            db = new JsonDbContext();
            _options = options;
        }
        // GET: api/<ResultsController>
        [HttpGet]
        public IEnumerable<Result> Get()
        {
            var jsonKey = _options.Value.AppKey;
            var request = Request;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);

            if (decodedKey==jsonKey)
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
