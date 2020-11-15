using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAir.CommonCore.Helper;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser, RegularUser")]

    public class ClientsController : Controller
    {
        #region atributos
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IMailHelper _mailHelper;
        private readonly IClientHelper _clientHelper;
        #endregion


        public ClientsController(IUserHelper userHelper, DataContext context, ICountryRepository countryRepository, IMailHelper mailHelper, IClientHelper clientHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _countryRepository = countryRepository;
            _mailHelper = mailHelper;
            _clientHelper = clientHelper;
        }
        public IActionResult Index() // Lista de todos os clientes
        {
            try
            {
                // Seleccionar todos os registos

                List<Client> Clients = _clientHelper.GetClients();

                return View(Clients);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        public async Task<IActionResult> IndexStatus() // Lista de todos os clientes
        {
            try
            {
                // Ir ao Historico dos clientes e seleccionar os Clientes para mudança de status, seja por validação directa devido a compra de milhas ou outros
                List<Historic_Status> validateChangeList = _context.Historic_Status.Where(x => x.isValidated == false).ToList();

                List<Client> ClientsFromHistoric = new List<Client>();

                foreach (var item in validateChangeList)
                {
                    Client client = await _clientHelper.GetClientByIdAsync(item.ClientId);
                    ClientsFromHistoric.Add(client);
                }

                //Clientes cuja data de verificação de Status é neste dia
                List<Client> ClientsFromDay = _clientHelper.GetClients().Where(x=>x.JoinDate.Value.Month == DateTime.Now.Month && x.JoinDate.Value.Day == DateTime.Now.Day).ToList();

                List<Client> clients = ClientsFromDay.Except(ClientsFromHistoric).Concat(ClientsFromHistoric.Except(ClientsFromDay)).ToList(); // Obter a lista total de clientes
                

                return View(clients);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        public async Task<IActionResult> Details(int id) // User ID 
        {
            try
            {
                // Obter o user

                Client client = await _clientHelper.GetClientByIdAsync(id);

                ClientViewModel model = new ClientViewModel();

                if (client == null)
                {
                    return NotFound();
                }

                model.client = client;

                model.Historic_Status = _context.Historic_Status.OrderBy(d => d.Start_Date).Where(x => x.ClientId == id).FirstOrDefault(); // Ordenado, estou a apanhar o primeiro registo
                model.Historic_StatusId = model.Historic_Status.Id;
                model.StatusId = model.Historic_Status.StatusId;
                model.Status = GetStatus();
                model.StatusName = _context.Status.Where(x => x.Id == model.StatusId).FirstOrDefault().Description;

                return View(model);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }



        private IEnumerable<SelectListItem> GetStatus()
        {

            var list = _context.Status.ToList();

            var statusList = list.Select(c => new SelectListItem
            {
                Text = c.Description,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            statusList.Insert(0, new SelectListItem
            {
                Text = "(Select a status...)",
                Value = "0"
            });

            return statusList;

        }


        public async Task<JsonResult> GetCitiesFromCountry(int? countryId)
        {
            if (countryId == 0)
            {
                Country country1 = new Country() { Id = 0, };
                return this.Json(country1);
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId.Value);
            return this.Json(country.Cities.OrderBy(c => c.Name));
        }



        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // Obter o cliente
                Client clientGet = await _clientHelper.GetClientByIdAsync(id);

                if (clientGet == null)
                {
                    return NotFound();
                }

                City city = await _countryRepository.GetCityAsync(clientGet.User.CityId);
                var country = _countryRepository.GetCountryAsync(city);

                var model = new ClientViewModel
                {
                    client = clientGet,

                };

                model.Countries = _countryRepository.GetComboCountries();

                model.client.FirstName = clientGet.FirstName;
                model.client.LastName = clientGet.LastName;
                model.client.Email = clientGet.User.Email;
                model.client.Client_Number = clientGet.Client_Number;
                model.client.StreetAddress = clientGet.StreetAddress;
                model.client.PostalCode = clientGet.User.PostalCode;
                model.CityId = clientGet.User.CityId;
                model.client.DateofBirth = (DateTime)clientGet.DateofBirth;
                model.client.TaxNumber = clientGet.User.TaxNumber;
                model.client.Identification = clientGet.User.Identification;
                model.client.JoinDate = (DateTime)clientGet.JoinDate;
                model.client.isClientNumberConfirmed = clientGet.isClientNumberConfirmed;
                model.client.PostalCode = clientGet.PostalCode;
                model.Status = GetStatus();


                model.Historic_Status = _context.Historic_Status.OrderBy(d => d.Start_Date).Where(x => x.ClientId == id).FirstOrDefault(); // Ordenado, estou a apanhar o primeiro registo
                model.Historic_StatusId = model.Historic_Status.Id;
                model.StatusId = model.Historic_Status.StatusId;
                model.Status = GetStatus();
                model.StatusName = _context.Status.Where(x => x.Id == model.StatusId).FirstOrDefault().Description;



                if (country != null)
                {
                    model.CountryId = country.Id;
                    model.Cities = await _countryRepository.GetComboCities(country.Id);
                    model.Countries = _countryRepository.GetComboCountries();
                    model.CityId = clientGet.User.CityId;
                }
                return View(model);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ClientViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Nunca me fio no que vem da View, fazer sempre o check com a base de dados. ( Fazer a pesquisa por nif, número de identificação e email). QQ um destes campos pode ter mudado, ou pode ter havido um engano
                // Obter o cliente
                Client client = await _clientHelper.GetClientByClientNumberAsync(model.client.Client_Number);

                if (client != null)
                {

                    if (client.isClientNumberConfirmed == false && model.client.isClientNumberConfirmed == true) // Administrador aceitou o número de cliente --> Enviar email com a confirmação
                    {
                        var response = UpdateClient(model); // Actualizar o Cliente

                        if (response) // Cliente actualizado com sucesso
                        {
                            // Enviar um eMail ao cliente com a confirmação                            

                            _mailHelper.SendMail(client.Email, "CinelAir Activated Account", $"<h1>CinelAir Client Number</h1>" +
                            $"Welcome onboard! Your client number is {client.Client_Number}");

                            ViewBag.Message = "The client was updated with sucess! Was sent an email with the client number!";
                            return View();
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Updated Error");
                    return View(model);

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");

                    return View(model);
                }
            }
            else
            {
                return NotFound();
            }
        }



        public bool UpdateClient(ClientViewModel model)
        {
            try
            {
                var client = _context.Client.Where(x => x.Client_Number == model.client.Client_Number).FirstOrDefault();
                client.FirstName = model.client.FirstName;
                client.LastName = model.client.LastName;
                client.Email = model.client.Email;
                client.Client_Number = model.client.Client_Number;
                client.StreetAddress = model.client.StreetAddress;
                client.PostalCode = model.client.PostalCode;
                client.User.CityId = model.CityId;
                client.DateofBirth = (DateTime)model.client.DateofBirth;
                client.TaxNumber = model.client.TaxNumber;
                client.Identification = model.client.Identification;
                client.JoinDate = (DateTime)model.client.JoinDate;
                client.isClientNumberConfirmed = model.client.isClientNumberConfirmed;

                _context.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IActionResult> EditSuperUser(int id)
        {
            // Obter o cliente
            Client clientGet = await _clientHelper.GetClientByIdAsync(id);

            if (clientGet == null)
            {
                return NotFound();
            }

            StatusViewModel model = new StatusViewModel();

            DateTime dateMiles = new DateTime(DateTime.Now.Year+2, model.client.JoinDate.Value.Month, model.client.JoinDate.Value.Day); //  ano que passou mais os dois anos de validade que faltam

            DateTime dateFlights = new DateTime(DateTime.Now.Year-1, model.client.JoinDate.Value.Month, model.client.JoinDate.Value.Day); //  Voos Realizados no último ano

            model.client = clientGet;

            model.miles_Status_Year = _context.Mile_Status.Where(x => x.ClientId == clientGet.Id && x.Validity >= dateMiles).Sum(x => x.Miles_Number); // Ir à tabela de milhas status e somar as milhas o cliente, apenas as que são válidas

            model.StatusId = _context.Historic_Status.Where(x => x.ClientId == clientGet.Id && x.End_Date == null).FirstOrDefault().StatusId; // Obter o status actual do cliente

            model.Statuses = GetStatus();      

            model.flights_Year = _context.Travel_Ticket.Where(x=>x.ClientId == clientGet.Id && x.Travel_Date >= dateFlights).Count(); // Ir à tabela de voos e obter o núemro de voos que o cliente realizou no último ano


            return View(model);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSuperUser(ClientViewModel model) 
        {
            // Obter o cliente
            Client clientGet = await _clientHelper.GetClientByIdAsync(model.client.Id);

            // Ir à tabela de historic Status e verificar qual o Status do cliente
            int clientOldStatusId = _context.Historic_Status.Where(x => x.ClientId == clientGet.Id).FirstOrDefault().StatusId;

            if (clientOldStatusId == model.StatusId) // Não houve alteração de estatuto
            {
                RedirectToAction("Index", "Clients"); //Accção, Controlador
            }

            // Houve alteração de estatudo. Assim, primeiro ir ao historico e colocar a End Date neste dia
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); //  ano que passou mais os dois anos de validade que faltam

            Historic_Status Old = _context.Historic_Status.Where(x => x.ClientId == clientGet.Id).FirstOrDefault();

            Old.End_Date = date;
            _context.Historic_Status.Update(Old);
            await _context.SaveChangesAsync();



            // Inserir um novo registo para o novo estado
            Historic_Status New = new Historic_Status()
            {
                ClientId = clientGet.Id,
                Start_Date = date,
                isValidated = true,
                StatusId = model.StatusId,
                wasNominated = false,
            };
            _context.Historic_Status.Add(New);
            await _context.SaveChangesAsync();


            ViewBag.Message = "Status updated";
            return NotFound();
        
        }
        
            

    }
}
