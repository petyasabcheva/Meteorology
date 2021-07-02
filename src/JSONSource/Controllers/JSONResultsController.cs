using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
        public IActionResult Get()
        {
            var jsonKey = _options.Value.AppKey;
            var request = Request;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result = db.Results.FirstOrDefault();
            var response = new ResultToReturn();
            if (decodedKey==jsonKey)
            {
                response.Temperature = result.Temperature;
                response.Pressure = result.Pressure;
                return Ok(response);
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
    }
}
