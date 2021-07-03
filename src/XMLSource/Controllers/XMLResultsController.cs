using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using XMLSource.Models;
using XMLSource.Services;

namespace XMLSource.Controllers
{
    [Route("/today")]
    [ApiController]
    public class XMLResultsController : ControllerBase
    {
        private XMLDbContext _db;
        private readonly IOptions<MyAppSettings> _options;

        public XMLResultsController(IOptions<MyAppSettings> options,XMLDbContext db)
        {
            this._db = db;
            _options = options;
        }

         
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
                //missing authorization header
                resultToReturn.Error = 1;
                return NotFound(SerializeObject(resultToReturn));

            }
           
            if (!IsWorkingNow())
            {
                //source is offline 
                resultToReturn.Error = 3;
                return NotFound(SerializeObject(resultToReturn));
            }

            var xmlKey = _options.Value.AppKey;
            var encodedKey = GetKeyFromRequest(request);
            var decodedKey = DecodeKey(encodedKey);
            var result = _db.Results.First();
           

            if (decodedKey == xmlKey)
            {
                //correct authorization header
                resultToReturn.Success = true;
                resultToReturn.Error = 0;
                resultToReturn.Data = new Models.Data() { Temperature = result.Temperature, Pressure = result.Pressure };

                return Ok(SerializeObject(resultToReturn));
            }
            if(decodedKey != xmlKey)
            {
                //wrong authorization header
                resultToReturn.Error = 1;
            }

            //unexpected error
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

        static bool IsWorkingNow()
        {
            var timeNow = DateTime.UtcNow;
            var startTimeToday = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 14, 30, 0);
            var endTimeToday = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 16, 20, 0);

            if (startTimeToday > timeNow || endTimeToday < timeNow)
            {
                return false;
            }

            return true;
        }

    }
}
