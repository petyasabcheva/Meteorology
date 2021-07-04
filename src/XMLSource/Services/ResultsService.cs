using System.Linq;
using XmlSource.Models;

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
