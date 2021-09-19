using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// New dependencies
using emsiproject.DataAccess;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace emsiproject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IDataHandler _dataHandler;
        private readonly ILogger<AreasController> _logger;
        public AreasController(IDataHandler dataHandler, ILogger<AreasController> logger)
        {
            _dataHandler = dataHandler;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Areas(string name, string abbr, string display_id)
        {
            var parameters = new { name = name, abbr = abbr, display_id = display_id };
            _logger.LogInformation("Predicate parameters recieved: {@Parameters} ", parameters);
            string jsonString = string.Empty;
            DataAccessResponse response = new DataAccessResponse();

            try
            {
                (jsonString, response) = _dataHandler.Search(name, abbr, display_id);

                if(!response.IsValid)
                {
                    foreach (string error in response.Errors)
                    {
                        _logger.LogError("Error in DataHandler(): {@Error}", error);
                    }
                }
                else
                {
                    _logger.LogInformation("Query Successful: " + response.IsValid.ToString() 
                        + " - " + response.RecordsReturned.ToString() + " records returned");
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Call to DataHandler.Search() Failed: " + e.Message);
                return StatusCode(500);
            }

            return jsonString;
        }
    }
}
