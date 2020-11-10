using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientHelper _clientHelper;

        public HomeController(ILogger<HomeController> logger, IClientHelper clientHelper)
        {
            _logger = logger;
            _clientHelper = clientHelper;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                // Seleccionar todos os registos

                List<Client> Clients = _clientHelper.GetClientsToValidate();

                return View(Clients);
                

            }

            return View();
        }

        public IActionResult Privacy(string id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
