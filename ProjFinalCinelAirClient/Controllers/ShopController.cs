using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Data.Repositories;
using ProjFinalCinelAirClient.Models;

namespace ProjFinalCinelAirClient.Controllers
{
    public class ShopController : Controller
    {
        private readonly IBuyMilesShopRepository _buyMilesShopRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMile_BonusRepository _mile_BonusRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHistoric_StatusRepository _historic_StatusRepository;
        private readonly IStatusRepository _statusRepository;

        public ShopController(IBuyMilesShopRepository buyMilesShopRepository, IClientRepository clientRepository,
                               IMile_BonusRepository mile_BonusRepository, ITransactionRepository transactionRepository,
                               IHistoric_StatusRepository historic_StatusRepository, IStatusRepository statusRepository)
        {
            _buyMilesShopRepository = buyMilesShopRepository;
            _clientRepository = clientRepository;
            _mile_BonusRepository = mile_BonusRepository;
            _transactionRepository = transactionRepository;
            _historic_StatusRepository = historic_StatusRepository;
            _statusRepository = statusRepository;
        }

        public IActionResult MileShopBuyMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var shop = _buyMilesShopRepository.GetAll().ToList();
            List<BuyMilesShop> sList = new List<BuyMilesShop>();

            //lista contém só milhas que o cliente pode transferir com base nas milhas já compradas ou transferidas
            foreach (var item in shop)
            {
                int aux = 20000 - client.AnnualMilesBought;
                if (item.MileQuantity <= aux)
                {
                    sList.Add(item);
                }
            }

            var model = new BuyMilesViewModel
            {
                Id = client.Id,
                Miles_Bonus = client.Miles_Bonus,
                ShopList = sList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MileShopBuyMiles(BuyMilesViewModel model)
        {
            var client = await _clientRepository.GetByIdAsync(model.Id);

            if (model.SelectedRadio < 1)
            {
                TempData["m"] = "Please choose an amount to purchase";

                return RedirectToAction("MileShopBuyMiles");
            }         
            
            var selectedItem = await _buyMilesShopRepository.GetByIdAsync(model.SelectedRadio);

            client.Miles_Bonus += selectedItem.MileQuantity;
            client.AnnualMilesBought += selectedItem.MileQuantity;

            var milesToAdd = new Mile_Bonus
            {
                ClientId = client.Id,
                Miles_Number = selectedItem.MileQuantity,
                Validity = DateTime.Now.AddYears(1),
                available_Miles_Bonus = client.Miles_Bonus,               
            };

            var transaction = new Transaction
            {
                ClientId = client.Id,
                Movement_Type = "Purchase",
                Date = DateTime.Now,
                Description = $"You bought {selectedItem.MileQuantity} Miles",
                Balance_Miles_Status = client.Miles_Status,
                Balance_Miles_Bonus = client.Miles_Bonus,
                Miles = selectedItem.MileQuantity
            };

            await _clientRepository.UpdateAsync(client);
            await _mile_BonusRepository.CreateAsync(milesToAdd);
            await _transactionRepository.CreateAsync(transaction);

            TempData["succ"] = $"Your purchase of {selectedItem.MileQuantity} miles has been complete!";

            return RedirectToAction("MileShopBuyMiles");
        }


        public IActionResult MilesShopExtendMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var shop = _buyMilesShopRepository.GetAll();
            List<BuyMilesShop> sList = new List<BuyMilesShop>();

            //lista contém só milhas que o cliente pode transferir com base nas milhas já compradas ou transferidas
            foreach (var item in shop)
            {
                int aux = 20000 - client.AnnualMilesExtended;
                if (item.MileQuantity <= aux && item.MileQuantity <= client.Miles_Bonus)
                {
                    sList.Add(item);
                }
            }

            var model = new ExtendMilesViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Miles_Bonus = client.Miles_Bonus,
                ShopList = sList
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> MileShopExtendMiles_Confirm(ExtendMilesViewModel model)
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var shop = _buyMilesShopRepository.GetAll();

            if (model.SelectedAmount == 0)
            {
                TempData["radio"] = "Please select an amount";

                return RedirectToAction("MileShopTransferMiles");
            };


            var clientBonus = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id).OrderBy(o => o.Validity).ToList();

            int difference = _clientRepository.DeductMilesWitCut(model.SelectedAmount,client ,clientBonus);


            var clientTransaction = new Transaction
            {
                ClientId = client.Id,
                Miles = model.SelectedAmount,
                Date = DateTime.Now,
                Movement_Type = "Transfer",
                Description = $"You extended {model.SelectedAmount} miles and lost {difference} miles",
                Balance_Miles_Bonus = client.Miles_Bonus,
                Balance_Miles_Status = client.Miles_Status
            };

            await _transactionRepository.CreateAsync(clientTransaction);

            return RedirectToAction("MilesShopExtendMiles");
        }


        public IActionResult MileShopTransferMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var clientList = _clientRepository.GetAll();
            var shopt = _buyMilesShopRepository.GetAll().ToList();
            List<BuyMilesShop> sList = new List<BuyMilesShop>();

            //lista contém só milhas que o cliente pode transferir com base nas milhas já compradas ou transferidas
            foreach (var item in shopt)
            {
                int aux = 20000 - client.AnnualMilesTransfered;
                if (item.MileQuantity <= aux && item.MileQuantity <= client.Miles_Bonus)
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
                AnnualMilesTransfered = client.AnnualMilesTransfered,
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
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);

            if (model.SelectedRadio == 0)
            {
                TempData["radio"] = "Please select an amount";

                return RedirectToAction("MileShopTransferMiles");
            };

            if (model.SelectedClientNumber == client.Client_Number.ToString())
            {
                TempData["c"] = "You cannot choose your own client number. Please pick a different client!";

                return RedirectToAction("MileShopTransferMiles");
            }


            if (selectedClient != null)
            {
          
                var selectedAmount = await _buyMilesShopRepository.GetByIdAsync(model.SelectedRadio);
                var clientBonus = _mile_BonusRepository.GetAll().Where(o => o.ClientId == client.Id).OrderBy(o => o.Validity).ToList();
                var selectedClientBonus = _mile_BonusRepository.GetAll().Where(u => u.ClientId == selectedClient.Id).OrderBy(u => u.Validity).ToList();              

                _clientRepository.DeductMilesWithoutCut(selectedAmount.MileQuantity, clientBonus);

                var firstClientItem = clientBonus.First();

                client.Miles_Bonus -= selectedAmount.MileQuantity;
                client.AnnualMilesTransfered += selectedAmount.MileQuantity;

                selectedClient.Miles_Bonus += selectedAmount.MileQuantity;
              

                var selectedClientMileBonus = new Mile_Bonus
                {
                    Miles_Number = selectedAmount.MileQuantity,
                    Validity = firstClientItem.Validity.AddYears(1),
                    available_Miles_Bonus = selectedClient.Miles_Bonus + selectedAmount.MileQuantity,
                    ClientId = selectedClient.Id
                };

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
                await _mile_BonusRepository.CreateAsync(selectedClientMileBonus);

                TempData["su"] = $"You transfered {selectedAmount.MileQuantity} miles to {selectedClient.FullName} with client number {selectedClient.Client_Number}";

                return RedirectToAction("MileShopTransferMiles");
            }
            else
            {
                TempData["t"] = "Selected Client does not exist!";

                return RedirectToAction("MileShopTransferMiles");
            }
        }


        public IActionResult MilesShopConvertMiles()
        {
            var client = _clientRepository.GetClientByUserEmail(User.Identity.Name);
            var shop = _buyMilesShopRepository.GetAll().ToList();            
            var status = _statusRepository.GetClientStatusById(client.Id);

            var selectedList = _clientRepository.ConvertMilesAmountSelection(status.Description, client, shop);

            var model = new ConvertMilesViewModel
            {
                Id = client.Id,
                ShopList = selectedList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MileShopConvertMiles_confirm(ConvertMilesViewModel model)
        {
            if (model.SelectedRadio == 0)
            {
                TempData["radio"] = "Please select an amount";

                return RedirectToAction("MilesShopConvertMiles");
            };

            var client = await _clientRepository.GetByIdAsync(model.Id);
    
            var clientTransaction = new Transaction
            {
                ClientId = client.Id,
                Miles = model.SelectedRadio,
                Date = DateTime.Now,
                Movement_Type = "Convert",
                Description = $"You converted {model.SelectedRadio} Bonus Miles to {(model.SelectedRadio/2)} Status Miles",
                Balance_Miles_Bonus = client.Miles_Bonus - model.SelectedRadio,
                Balance_Miles_Status = client.Miles_Status + (model.SelectedRadio/2)
            };

            await _transactionRepository.CreateAsync(clientTransaction);

            TempData["succ"] = $"You have successfully converted {model.SelectedRadio} Bonus Miles to {(model.SelectedRadio / 2)} Status Miles!";


            return RedirectToAction("MilesShopConvertMiles");

        }


     
    }
}
