using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// New dependencies
using emsiproject.Models;
using Microsoft.EntityFrameworkCore;

namespace emsiproject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly AreasContext _context;

        public AreasController(AreasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        {
            return await _context.Areas.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas(string name)
        {
            return await _context.Areas.Where(x=>x.Name == name).ToListAsync();
        }
    }
}
