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
        private readonly ITravel_TicketRepository _travel_TicketRepository;
        private readonly IBuyMilesShopRepository _buyMilesShopRepository;

        public ClientAreaController(IClientRepository clientRepository, IUserHelper userHelper,
                                    IMile_BonusRepository mile_BonusRepository, IMile_StatusRepository mile_StatusRepository,
                                    ITransactionRepository transactionRepository, INotificationRepository notificationRepository,
                                    IHistoric_StatusRepository historic_StatusRepository, IStatusRepository statusRepository,
                                    ITravel_TicketRepository travel_TicketRepository, IBuyMilesShopRepository buyMilesShopRepository)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _mile_BonusRepository = mile_BonusRepository;
            _mile_StatusRepository = mile_StatusRepository;
            _transactionRepository = transactionRepository;
            _notificationRepository = notificationRepository;
            _historic_StatusRepository = historic_StatusRepository;
            _statusRepository = statusRepository;
            _travel_TicketRepository = travel_TicketRepository;
            _buyMilesShopRepository = buyMilesShopRepository;
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



        public async Task<IActionResult> UpgradeWithMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var ticket = _travel_TicketRepository.GetAll().Where(o => o.ClientId == client.Id);

            var model = new UpgradeWithMilesViewModel
            {

            };

            return View();
        }


        public IActionResult MileShopBuyMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var ticket = _travel_TicketRepository.GetAll().Where(o => o.ClientId == client.Id);
            var shop = _buyMilesShopRepository.GetAll();

            var model = new BuyMilesViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Miles_Bonus = client.Miles_Bonus,
                ShopList = shop.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MileShopBuyMiles(int id)
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var shop = await _buyMilesShopRepository.GetByIdAsync(id);

            //TODO: amanha há mais

            return View();
        }


        public IActionResult MileShopExtendMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var ticket = _travel_TicketRepository.GetAll().Where(o => o.ClientId == client.Id);
            var shop = _buyMilesShopRepository.GetAll();

            var model = new BuyMilesViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Miles_Bonus = client.Miles_Bonus,
                ShopList = shop.ToList()
            };

            return View(model);
        }


        public IActionResult MileShopTransferMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var ticket = _travel_TicketRepository.GetAll().Where(o => o.ClientId == client.Id);
            var clientList = _clientRepository.GetAll();
            var shopt = _buyMilesShopRepository.GetAll().ToList();                                                        
            List<BuyMilesShop> sList = new List<BuyMilesShop>();

            //lista contém só milhas que o cliente pode transferir com base nas milhas já compradas ou transferidas
            foreach(var item in shopt)
            {
                int aux = 20000 - client.AnnualMilesShopped;
                if (item.MileQuantity <= aux)
                {
                    sList.Add(item);
                }
            }

            var rmodel = new TransferMilesViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Miles_Bonus = client.Miles_Bonus,
                AnnualMilesShopped = client.AnnualMilesShopped,
                ClientList = clientList.ToList(),
                ShopList = sList,
                Message = ""
            };

            return View(rmodel);
        }


        [HttpPost]
        public async Task<IActionResult> MileShopTransferMiles_Confirm(TransferMilesViewModel model)
        {
            var selectedClient = _clientRepository.GetClientByClientNumber(Convert.ToInt32(model.SelectedClientNumber));

            if (model.SelectedRadio == 0)
            {
                TempData["radio"] = "Please select an amount";

                return RedirectToAction("MileShopTransferMiles");
            };

            if (selectedClient != null)
            {
                var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
                var selectedAmount = await _buyMilesShopRepository.GetByIdAsync(model.SelectedRadio);
                var clientBonus = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id);
                var selectedClientBonus = _mile_BonusRepository.GetAll().Where(u => u.ClientId == selectedClient.Id);

                client.Miles_Bonus -= selectedAmount.MileQuantity;
                //TODO: falar com dulce sobre Miles_Bonus
                selectedClient.Miles_Bonus += selectedAmount.MileQuantity;
                var clientTransaction = new Transaction
                {
                    ClientId = client.Id,
                    Miles = selectedAmount.MileQuantity,
                    Date = DateTime.Now,
                    Movement_Type = "Transfer",
                    Description = $"You transfered {selectedAmount.MileQuantity} to {selectedClient.FirstName} with client number {selectedClient.Client_Number}",
                    Balance_Miles_Bonus = client.Miles_Bonus - selectedAmount.MileQuantity,
                    Balance_Miles_Status = client.Miles_Status
                };
                var selectedClientTransaction = new Transaction
                {
                    ClientId = selectedClient.Id,
                    Miles = selectedAmount.MileQuantity,
                    Date = DateTime.Now,
                    Movement_Type = "Transfer",
                    Description = $"You were transfered {selectedAmount.MileQuantity} by {client.FirstName} with client number {client.Client_Number}",
                    Balance_Miles_Bonus = selectedClient.Miles_Bonus + selectedAmount.MileQuantity,
                    Balance_Miles_Status = client.Miles_Status
                };

                await _clientRepository.UpdateAsync(client);
                await _clientRepository.UpdateAsync(selectedClient);
                await _transactionRepository.CreateAsync(clientTransaction);
                await _transactionRepository.CreateAsync(selectedClientTransaction);

                TempData["t"] = "Transfer completed sucessfully!";
                //TODO: redirecionar para a mesma página mandar mensagem de success
                return RedirectToAction("MileShopTransferMiles");               
            }
            else
            {
                TempData["t"] = "Selected Client does not exist!";

                return RedirectToAction("MileShopTransferMiles");
            }          
        }



    }
}
