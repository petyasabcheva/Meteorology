using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSource.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JSONSource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSONResultsController : ControllerBase
    {
        private JSONDbContext db;
        public JSONResultsController()
        {
            db = new JSONDbContext();
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
