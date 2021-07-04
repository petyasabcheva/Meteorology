using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XMLSource.Models;

namespace XmlSource.Services
{
    public class ResultsService :IResultsService
    {
        private readonly XmlDbContext _db;

        public ResultsService(XmlDbContext db)
        {
            this._db = db;
        }

        public Result GetToday()
        {
            var result = _db.Results.First();
            return result;
        }
    }
}
