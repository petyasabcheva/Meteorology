using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSource.Models;

namespace JsonSource.Services
{
    public class ResultsService:IResultsService
    {
        private readonly JsonDbContext _db;

        public ResultsService(JsonDbContext db)
        {
            this._db = db;
        }

        public Result GetToday ()
        {
            var result = _db.Results.First();
            return result;
        }
    }
}
