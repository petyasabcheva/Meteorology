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
    [Route("/today")]
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
            var resultToReturn = new ResultToReturn
            {
                Success = false,
                Error = 2
            };
            var request = Request;
            if (!request.Headers.Any(x => x.Key == "Authorization"))
            {
                resultToReturn.Error = 1;
                return NotFound(SerializeObject(resultToReturn));

            }
            var workingTime = GetWorkingTime();
            var timeNow = DateTime.UtcNow;
            if (workingTime[0] > timeNow || workingTime[1] < timeNow)
            {
                resultToReturn.Error = 3;
                return NotFound(SerializeObject(resultToReturn));
            }

            var xmlKey = _options.Value.AppKey;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result = db.Results.FirstOrDefault();
           

            if (decodedKey == xmlKey)
            {
                resultToReturn.Success = true;
                resultToReturn.Error = 0;
                resultToReturn.Data = new Data.Data() { Temperature = result.Temperature, Pressure = result.Pressure };

                return Ok(SerializeObject(resultToReturn));
            }
            else
            {
                resultToReturn.Success = false;
                resultToReturn.Error = 1;
            }

            return NotFound(SerializeObject(resultToReturn));
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

        public static string SerializeObject(ResultToReturn toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        static List<DateTime> GetWorkingTime()
        {
            var dateToday = DateTime.UtcNow;
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
