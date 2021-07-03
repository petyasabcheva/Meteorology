using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using JSONSource.Models;
using JSONSource.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JSONSource.Controllers
{
    [Route("/today")]
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
            var request = Request;
            if (!request.Headers.Any(x => x.Key == "Authorization"))
            {
                //no authorization header
                return new ObjectResult(new ResultInformation() { Message = "Unauthorized", StatusCode = 401 });

            }

            if (!IsWorkingNow())
            {
                //source is offline 
                return new ObjectResult(new ResultInformation() {Message = "Offline",StatusCode = 504});
            }
            var jsonKey = _options.Value.AppKey;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result = db.Results.First();
            var response = new ResultToReturn();

            if (decodedKey == jsonKey)
            {
                //correct authorization header
                response.Temperature = result.Temperature;
                response.Pressure = result.Pressure;
                return Ok(response);
            }
            if (decodedKey != jsonKey)
            {
                //wrong authorization header
                return new ObjectResult(new ResultInformation() { Message = "Unauthorized", StatusCode = 401 });

            }

            //unexpected error
            return new ObjectResult(new ResultInformation() { Message = "Unexpected error", StatusCode = 500 });
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

        static bool IsWorkingNow()
        {
            var  timeNow = DateTime.UtcNow;
            var startTimeToday = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 8, 30, 0);
            var endTimeToday = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 14, 20, 0);

            if (startTimeToday > timeNow || endTimeToday < timeNow)
            {
                return false;
            }

            return true;
        }
    }
}
