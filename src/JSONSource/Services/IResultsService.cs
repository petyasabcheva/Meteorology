using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSource.Models;

namespace JsonSource.Services
{
    public interface IResultsService
    {
        Result GetToday();
    }
}
