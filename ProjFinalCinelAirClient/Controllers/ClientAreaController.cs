using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAirClient.Data.Repositories;
using ProjFinalCinelAirClient.Helpers;

namespace ProjFinalCinelAirClient.Controllers
{
    public class ClientAreaController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;

        public ClientAreaController(IClientRepository clientRepository, IUserHelper userHelper)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View();
        }


       /* public async Task<IActionResult> Miles_Shop()
        {
            
        }


        public async Task<IActionResult> BalanceAndMovements()
        {

        }


        public async Task<IActionResult> UpgradeWithMiles()
        {

        }


        public async Task<IActionResult> Partners()
        {

        }


        public async Task<IActionResult> CinelAir_Card()
        {

        }


        public async Task<IActionResult> Nomenate_Gold()
        {

        }


        public async Task<IActionResult> Donate_Miles()
        {

        }*/
    }
}
