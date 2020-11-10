using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAirAdmin.Data.Repositories;

namespace ProjFinalCinelAirAdmin.Controllers
{
    public class AwardTicketController : Controller
    {

        private readonly IAwardTicketRepository _awardTicketRepository;



        public AwardTicketController(IAwardTicketRepository awardTicketRepository)
        {
            _awardTicketRepository = awardTicketRepository;
        }


        public IActionResult Index()
        {
            var list = _awardTicketRepository.GetAll().ToList();
            return View(list);
            
        }
    }
}
