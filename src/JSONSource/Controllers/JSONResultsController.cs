using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using JsonSource.Models;
using JsonSource.Services;
using Microsoft.Extensions.Options;
using IKeyManager = JsonSource.Services.IKeyManager;

namespace JsonSource.Controllers
{
    [Route("/today")]
    [ApiController]
    public class JsonResultsController : ControllerBase
    {
        private readonly IOptions<MyAppSettings> _options;
        private readonly IResultsService _resultsService;
        private readonly IKeyManager _keyManager;

        public JsonResultsController(IOptions<MyAppSettings> options, IResultsService resultsService, IKeyManager keyManager)
        {
            this._resultsService = resultsService;
            this._options = options;
            this._keyManager = keyManager;

        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var request = Request;
            if (!request.Headers.Any(x => x.Key == "Authorization"))
            {
                //no authorization header
                return new ObjectResult(new ResultInformation() { Message = "Unauthorized", StatusCode = 401 })
                {
                    StatusCode = 401
                };

            }

            if (!IsWorkingNow())
            {
                //source is offline 
                return new ObjectResult(new ResultInformation() { Message = "Offline", StatusCode = 504 })
                {
                    StatusCode = 504
                };
            }
            var jsonKey = _options.Value.AppKey;
            var result = this._resultsService.GetToday();
            var encodedKey = _keyManager.GetKeyFromRequest(request);
            var encodedOriginalKey = _keyManager.EncodeKey(jsonKey);
            var response = new ResultToReturn();

            if (encodedOriginalKey == encodedKey)
            {
                //correct authorization header
                response.Temperature = result.Temperature;
                response.Pressure = result.Pressure;
                return Ok(response);
            }
            if (encodedOriginalKey != encodedKey)
            {
                //wrong authorization header
                return new ObjectResult(new ResultInformation() { Message = "Unauthorized", StatusCode = 401 }) { StatusCode = 401 };

            }

            //unexpected error
            return new ObjectResult(new ResultInformation() { Message = "Unexpected error", StatusCode = 500 });
        }

        static bool IsWorkingNow()
        {
            var timeNow = DateTime.UtcNow;
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
