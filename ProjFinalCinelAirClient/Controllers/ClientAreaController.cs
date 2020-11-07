using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAirClient.Data.Repositories;
using ProjFinalCinelAirClient.Helpers;
using ProjFinalCinelAirClient.Models;

namespace ProjFinalCinelAirClient.Controllers
{
    public class ClientAreaController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly IMile_BonusRepository _mile_BonusRepository;
        private readonly IMile_StatusRepository _mile_StatusRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHistoric_StatusRepository _historic_StatusRepository;
        private readonly IStatusRepository _statusRepository;

        public ClientAreaController(IClientRepository clientRepository, IUserHelper userHelper,
                                    IMile_BonusRepository mile_BonusRepository, IMile_StatusRepository mile_StatusRepository,
                                    ITransactionRepository transactionRepository, INotificationRepository notificationRepository,
                                    IHistoric_StatusRepository historic_StatusRepository, IStatusRepository statusRepository)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _mile_BonusRepository = mile_BonusRepository;
            _mile_StatusRepository = mile_StatusRepository;
            _transactionRepository = transactionRepository;
            _notificationRepository = notificationRepository;
            _historic_StatusRepository = historic_StatusRepository;
            _statusRepository = statusRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Notifications()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);

            var notificationList = _notificationRepository.GetAll().Where(o => o.ClientId == client.Id);

            var model = new NotificationsViewModel
            {
                NotificationList = notificationList.ToList(),
            };

            return View(model);
        }


        public IActionResult BalanceAndTransactions()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);

            //soma de todas as milhas e a ultima data de expiration
            //maybe i don't need this(ou tiro os valores pelo cliente ou pelas tables de status e bonus)
            var bonus_miles = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id && o.Validity >= DateTime.Now);
            var status_miles = _mile_StatusRepository.GetAll().Where(o => o.ClientId == client.Id && o.Validity >= DateTime.Now);
            
            var totalstatus_miles = status_miles.Sum(i => i.available_Miles_Status);
            var totalbonus_miles = bonus_miles.Sum(i => i.available_Miles_Bonus);

            //estou a utilizar este
            var clientMilesBonus = client.Miles_Bonus;
            var clientMilesStatus = client.Miles_Status;

            var transactions = _transactionRepository.GetAll().Where(o => o.ClientId == client.Id);

            var model = new BalanceViewModel
            {
                available_Miles_Status = clientMilesStatus,
                available_Miles_Bonus = clientMilesBonus,
                ClientId = client.Id,
                TransactionList = transactions.ToList(),
                //levar ultima validade
            };

            return View(model);
        }

        /*public async Task<IActionResult> Nominate_Gold()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var status = _statusRepository.GetClientStatusById(client.Id);

            var clientList = _clientRepository.GetAll().ToList();

            var model = new NominateGoldViewModel
            {

            };
        }*/


        /*public async Task<IActionResult> UpgradeWithMiles()
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
