using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Helpers;

namespace ProjFinalCinelAirClient.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilesStatusController : ControllerBase
    {
        private readonly DataContext _context;

        public MilesStatusController(DataContext context)
        {
            _context = context;
            var list = _context.Users.ToList();
           
        }

        [HttpGet]
        public IActionResult GetMilesExtract(int id) 
        {
            try
            {
                var list = _context.Users.ToList();

                var client = _context.Client.Where(x => x.Id == id).FirstOrDefault();

                if (client != null)
                {
                    return Ok(_context.Mile_Status
                        .Where(x => x.ClientId == id)
                        .OrderBy(x => x.Validity));

                }
            }

            catch (Exception)
            {

                return NotFound();
            }
          
            

            return NotFound();
        
        }
    }
}
