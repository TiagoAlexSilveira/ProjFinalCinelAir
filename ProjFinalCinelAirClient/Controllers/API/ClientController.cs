using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAir.CommonCore.Data;

namespace ProjFinalCinelAirClient.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly DataContext _context;


        public ClientController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetClients()
        {
            return Ok(_context.Client);
                
        }
    }
}
