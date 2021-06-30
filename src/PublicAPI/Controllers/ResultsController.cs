using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly HttpClient client;
        private string JSONUrl = "http://localhost:58270/api/jsonresults/1";
        private string XMLUrl = "http://localhost:43529/api/xmlresults/1";

        public ResultsController()
        {
            client = new HttpClient();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var finalResult = new StringBuilder();
            finalResult.AppendLine("This is the result from the first source in JSON");
            var JSONResult = "";
            HttpResponseMessage JSONResponse = await client.GetAsync(JSONUrl);
            if (JSONResponse.IsSuccessStatusCode)
            {
                JSONResult = await JSONResponse.Content.ReadAsStringAsync();
            }

            finalResult.AppendLine(JSONResult);
            finalResult.AppendLine("This is the result from the second source in XML");
            var XMLResult = "";
            HttpResponseMessage XMLResponse = await client.GetAsync(XMLUrl);
            if (XMLResponse.IsSuccessStatusCode)
            {
                XMLResult = await XMLResponse.Content.ReadAsStringAsync();
            }

            finalResult.AppendLine(XMLResult);
            return finalResult.ToString().TrimEnd();
        }

    }
}
