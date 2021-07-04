using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using XMLSource.Models;
using XmlSource.Services;
using XMLSource.Services;

namespace XMLSource.Controllers
{
    [Route("/today")]
    [ApiController]
    public class XmlResultsController : ControllerBase
    {
        private readonly IResultsService _resultsService;
        private readonly IOptions<MyAppSettings> _options;
        private readonly IKeyManager _keyManager;


        public XmlResultsController(IOptions<MyAppSettings> options, IResultsService resultsService, IKeyManager keyManager)
        {
            this._resultsService = resultsService;
            _options = options;
            this._keyManager = keyManager;

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
            var encodedKey = _keyManager.GetKeyFromRequest(request);
            var encodedOriginalKey = _keyManager.EncodeKey(xmlKey);
            var result = this._resultsService.GetToday();
           

            if (encodedKey == encodedOriginalKey)
            {
                //correct authorization header
                resultToReturn.Success = true;
                resultToReturn.Error = 0;
                resultToReturn.Data = new Models.Data() { Temperature = result.Temperature, Pressure = result.Pressure };

                return Ok(SerializeObject(resultToReturn));
            }
            if(encodedKey != encodedOriginalKey)
            {
                //wrong authorization header
                resultToReturn.Error = 1;
            }

            //unexpected error
            return NotFound(SerializeObject(resultToReturn));
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
            var endTimeToday = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 22, 20, 0);

            if (startTimeToday > timeNow || endTimeToday < timeNow)
            {
                return false;
            }

            return true;
        }

    }
}
