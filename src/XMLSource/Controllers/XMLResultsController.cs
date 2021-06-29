using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XMLSource.Data;

namespace XMLSource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XMLResultsController : ControllerBase
    {
        private XMLDbContext db;
        public XMLResultsController()
        {
            db = new XMLDbContext();
        }
        // GET: api/<ResultsController>
        [HttpGet]
        public IEnumerable<Result> Get()
        {
            return db.Results.ToList();
        }

        // GET api/<ResultsController>/5
        [HttpGet("{id}")]
        public Result Get(int id)
        {
            return db.Results.FirstOrDefault(x => x.Id == id);
        }
    }
}
