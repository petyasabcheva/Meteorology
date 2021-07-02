using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSource.Data;
using JSONSource.Services;
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
        public IEnumerable<Result> Get(string encodedKey)
        {
            var jsonKey = _options.Value.AppKey;
            var base64EncodedBytes = System.Convert.FromBase64String(encodedKey);
            var decodedKey= System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            if (decodedKey==jsonKey)
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
