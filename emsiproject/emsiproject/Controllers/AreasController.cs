using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// New dependencies
using System.Data.SQLite;
using emsiproject.DataAccess;

namespace emsiproject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IDataHandler _dataHandler;
        public AreasController(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        //{
        //    return await _context.Areas.ToListAsync();
        //}

        [HttpGet]
        //[Route("")]
        //[Route("{q}")]
        //[Route("{q}/{r?}")]
        public ActionResult<string> Areas(string name, string abbr, string display_id)
        {
            try
            
            {
                return _dataHandler.Search(name, abbr, display_id);

            }
            catch(Exception e)
            {
                // Log
                return NotFound();
            }
        }
    }
}
