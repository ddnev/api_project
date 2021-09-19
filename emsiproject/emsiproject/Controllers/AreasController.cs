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

        [HttpGet("{predicate}")]
        public ActionResult<string> GetAreas(string predicate)
        {
            return _dataHandler.Search(predicate);
        }
    }
}
