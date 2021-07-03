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
            var workingTime = GetWorkingTime();
            var timeNow = DateTime.UtcNow;
            if (workingTime[0]>timeNow||workingTime[1]<timeNow)
            {
                return new ObjectResult(new ResultInformation() {Message = "Offline",StatusCode = 504});
            }
            var jsonKey = _options.Value.AppKey;
            var request = Request;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result = db.Results.FirstOrDefault();
            var response = new ResultToReturn();
            if (decodedKey == jsonKey)
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

        static List<DateTime> GetWorkingTime()
        {
            var  dateToday = DateTime.UtcNow;
            var startTimeToday = new DateTime(dateToday.Year, dateToday.Month, dateToday.Day, 10, 30, 0);
            var endTimeToday = new DateTime(dateToday.Year, dateToday.Month, dateToday.Day, 14, 20, 0);
            return new List<DateTime>
            {
                startTimeToday,
                endTimeToday
            };
        }
    }
}
