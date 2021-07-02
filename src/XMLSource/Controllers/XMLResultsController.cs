using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IEnumerable<Result> Get(string encodedKey)
        {
            var xmlKey = _options.Value.AppKey;
            var base64EncodedBytes = System.Convert.FromBase64String(encodedKey);
            var decodedKey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            if (decodedKey == xmlKey)
            {
                return db.Results.ToList();
            }

            return null;
        }

        // GET api/<ResultsController>/5
        [HttpGet("{id}")]
        public Result Get(int id)
        {
            return db.Results.FirstOrDefault(x => x.Id == id);
        }
    }
}
