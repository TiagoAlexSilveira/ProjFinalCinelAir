using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAir.CommonCore.Data.Entities;
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


        public IActionResult Nominate_Gold()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            //var status = _statusRepository.GetClientStatusById(client.Id);

            var clientList = _clientRepository.GetAll().ToList();
            var historic_status = _historic_StatusRepository.GetAll().ToList();
            List<Client> NotgoldClientList = new List<Client>();

            //get all clients with gold status
            foreach (var cl in clientList)
            {
                foreach (var hs in historic_status)
                {
                    if (hs.ClientId == cl.Id)
                    {
                        if (hs.StatusId == 2 || hs.StatusId == 3)
                        {
                            NotgoldClientList.Add(cl);
                        }
                    }                
                }
            }

            var model = new NominateGoldViewModel
            {
                ClientList = NotgoldClientList
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Nominate_Gold2(string searchInput)
        {
            ViewData["GetSearch"] = searchInput;

            var clients = _clientRepository.GetAllClientsWithStatusBasicOrSilver().AsQueryable();

            if (!String.IsNullOrEmpty(searchInput))
            {               
                clients = clients.Where(o => o.FirstName.Contains(searchInput) || o.Client_Number.ToString().Contains(searchInput));
            }

            var vmodel = new NominateGoldViewModel
            {
                ClientList = clients.ToList()
            };
            return View("Nominate_Gold", vmodel);
            
        }


        public async Task<IActionResult> Nominate_Gold_Confirm(int Id)
        {
            //var client = await _clientRepository.GetByIdAsync(id);
            var hstatus = _historic_StatusRepository.GetClientHistoric_StatusById(Id);

            hstatus.StatusId = 1;
            hstatus.Start_Date = DateTime.Now;
            hstatus.End_Date = hstatus.Start_Date.AddYears(1);
            hstatus.wasNominated = true;
            //TODO: fazer com que o user que nomeou não consiga nomear outra vez
            //TODO: mandar notificação a este user que recebeu o status gold

            await _historic_StatusRepository.UpdateAsync(hstatus);

            //TODO: apresentar mensagem nomenation sucessful
            return RedirectToAction("Index");

        }


        public IActionResult Donate_Miles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);

            //TODO: lista de companhias para donativos
            //var companyDonationList = _donationRepository

            /*var model = new DonationViewModel
            {

            };*/

            return View();
        }


        public IActionResult Partners()
        {
            //TODO: fazer partners repository, model e view
           return View();
        }



        public async Task<IActionResult> CinelAir_Card()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var hstatus = _historic_StatusRepository.GetClientHistoric_StatusById(client.Id);

            //TODO: fazer card repository, acabar model e view
            var model = new CinelAirCardViewModel
            {

            };

            return View();
        }



        /*public async Task<IActionResult> UpgradeWithMiles()
        {

        }


       

*/



    }
}
