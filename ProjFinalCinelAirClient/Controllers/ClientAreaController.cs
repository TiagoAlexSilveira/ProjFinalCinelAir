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
        private readonly IPartnerRepository _partnerRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IAwardTicketRepository _awardTicketRepository;

        public ClientAreaController(IClientRepository clientRepository, IUserHelper userHelper,
                                    IMile_BonusRepository mile_BonusRepository, IMile_StatusRepository mile_StatusRepository,
                                    ITransactionRepository transactionRepository, INotificationRepository notificationRepository,
                                    IHistoric_StatusRepository historic_StatusRepository, IStatusRepository statusRepository,
                                    ITravel_TicketRepository travel_TicketRepository, IBuyMilesShopRepository buyMilesShopRepository,
                                    IPartnerRepository partnerRepository, ICardRepository cardRepository, 
                                    IAwardTicketRepository awardTicketRepository)
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
            _partnerRepository = partnerRepository;
            _cardRepository = cardRepository;
            _awardTicketRepository = awardTicketRepository;
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
            var allMilesBonusList = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id).OrderBy(u => u.Validity).ToList();
            var allMilesListFirst = new Mile_Bonus();
            if (allMilesBonusList.Count > 0)
            {
                allMilesListFirst = allMilesBonusList.First(); //ultimo bloco de milhas com data de validade mais perto de expirar
            }           

            var transactions = _transactionRepository.GetAll().Where(o => o.ClientId == client.Id);
            var historicClient = _historic_StatusRepository.GetClientHistoric_StatusById(client.Id);

            var model = new BalanceViewModel
            {
                available_Miles_Status = client.Miles_Status,
                available_Miles_Bonus = client.Miles_Bonus,
                ClientId = client.Id,
                TransactionList = transactions.ToList(),
                ExpiryDateLastMiles = allMilesListFirst.Validity.ToShortDateString(),
                LastMiles = allMilesListFirst.Miles_Number,
                NextClientUpdate = historicClient.End_Date.ToString() //TODO: ver onde a dulce guarda a data de update do client
            };

            return View(model);
        }


        public IActionResult Nominate_Gold()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var clientHistoric_status = _historic_StatusRepository.GetClientHistoric_StatusById(client.Id);


            var model = new NominateGoldViewModel
            {
                ClientList = new List<Client>()
            };

            if (clientHistoric_status.wasNominated == true)
            {        
                TempData["cant"] = "You already nominated a client or have been nominated by another client";

                return View(model);
            }

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

            if (clients.ToList().Count == 0)
            {
                TempData["message"] = "There are no clients with that Name or Number";
            }

            //return View("Nominate_Gold", vmodel);
            return RedirectToAction("Nominate_Gold", vmodel);
        }


        public async Task<IActionResult> Nominate_Gold_Confirm(int Id)
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var hstatus = _historic_StatusRepository.GetClientHistoric_StatusById(Id);
            var selectedClient = await _clientRepository.GetByIdAsync(hstatus.Id);

            hstatus.StatusId = 1;
            hstatus.Start_Date = DateTime.Now;
            hstatus.End_Date = hstatus.Start_Date.AddYears(1);
            hstatus.wasNominated = true;

            var clientNotification = new Notification
            {
                ClientId = client.Id,
                Date = DateTime.Now,
                Subject = "Nominate Gold",
                Message = $"You have nominated {selectedClient.FullName} with client number {selectedClient.Client_Number} for gold status "
            };

            var selectedClientNotification = new Notification
            {
                ClientId = hstatus.ClientId,
                Date = DateTime.Now,
                Subject = "Updated Status",
                Message = $"You have been nominated for gold status by {client.FullName} with client number {client.Client_Number}." +
                $"You can now enjoy all the benefits that come with gold status " +
                $"From this point forward you may not nominate another client for gold status"               
            };
            //TODO: Mandar Email??
            await _historic_StatusRepository.UpdateAsync(hstatus);
            await _notificationRepository.CreateAsync(clientNotification);
            await _notificationRepository.CreateAsync(selectedClientNotification);

            TempData["sc"] = $"You have sucessfully nominated {selectedClient.FullName} for status Gold"; 

            return RedirectToAction("Nominate_Gold");

        }


        public IActionResult Donations()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var companyDonationList = _partnerRepository.GetAll().Where(o => o.isCharity == true && o.isValidated == true);
            var shoplist = _buyMilesShopRepository.GetAll();
            List<BuyMilesShop> sList = new List<BuyMilesShop>();

            foreach (var item in shoplist)
            {
                if (client.Miles_Bonus >= item.MileQuantity)
                {
                    sList.Add(item);
                }
            }

            var model = new DonationViewModel
            {
                Id = client.Id,
                DonationList = companyDonationList.ToList(),
                ShopList = sList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Donations_Confirm(DonationViewModel model)
        {
            if (model.SelectedItem < 1|| model.SelectedPartner < 1)
            {
                TempData["donation"] = "Please choose an institution and an amount to donate!";

                return RedirectToAction("Donations");
            }

            var client = await _clientRepository.GetByIdAsync(model.Id);
            var selectedDonation = await _partnerRepository.GetByIdAsync(model.SelectedPartner);
            var milesBonus = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id).OrderBy(u => u.Validity).ToList();

            client.Miles_Bonus -= model.SelectedItem;

            _clientRepository.DeductMilesWithoutCut(model.SelectedItem, milesBonus);
           

            var clientTransaction = new Transaction
            {
                ClientId = client.Id,
                Miles = model.SelectedItem,
                Date = DateTime.Now,
                Movement_Type = "Donation",
                Description = $"You donated {model.SelectedItem} to {selectedDonation.Name}",
                Balance_Miles_Bonus = client.Miles_Bonus - model.SelectedItem,
                Balance_Miles_Status = client.Miles_Status
            };

            await _clientRepository.UpdateAsync(client);
            await _transactionRepository.CreateAsync(clientTransaction);

            //TODO mandar mail?
            TempData["donation"] = $"You have donated {model.SelectedItem} Miles to {selectedDonation.Name}.Thank you for your donation!";

            return RedirectToAction("Donations");
        }


        public IActionResult Partners()
        {
            var partners = _partnerRepository.GetAll().Where(o => o.isValidated == true & o.isCharity == false);

            var model = new PartnersViewModel
            {
                PartnerList = partners.ToList()
            };

            return View(model);
        }



        public async Task<IActionResult> CinelAirCard()
        {
            //var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            //var hstatus = _historic_StatusRepository.GetClientHistoric_StatusById(client.Id);
            //var mileList = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id).OrderBy(o => o.Validity).ToList();
            ////var lastMiles = mileList.First();
            //var clientStatus = _statusRepository.GetClientStatusById(client.Id);
                
            //var card = await _cardRepository.GetByIdAsync(client.CardId);
            //var card1 = await _cardRepository.GetByIdAsync(1);

            //var model = new CinelAirCardViewModel
            //{
            //    StatusId = hstatus.StatusId,
            //    ClientStatus = clientStatus.Description,
            //    Id = client.Id,
            //    Miles_Bonus = client.Miles_Bonus,
            //    Miles_Status = client.Miles_Status,
            //    //Miles_Number = lastMiles.Miles_Number,
            //    CardId = card.Id,
            //    ExpirationDate = card.ExpirationDate
            //};

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



        public IActionResult Prizes()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var awardsList = _awardTicketRepository.GetAll().ToList();

            var model = new AwardTicketViewModel
            {
                awardsList = awardsList,
                Id = client.Id
            };

            return View(model);
        }

        public async Task<IActionResult> Prizes_Confirm(AwardTicketViewModel model)
        {
            var client = await _clientRepository.GetByIdAsync(model.Id);
            
            //TODO: client tem de guardar o prémio dele algures

            

            return View(model);
        }

    }
}
