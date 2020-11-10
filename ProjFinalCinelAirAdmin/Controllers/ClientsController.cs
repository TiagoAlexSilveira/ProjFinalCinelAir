using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
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

        public async Task<IActionResult> Details(int id) // User ID 
        {
            try
            {
                // Obter o user

                Client client = await _clientHelper.GetClientByIdAsync(id);

                if (client == null)
                {
                    return NotFound();
                }

                return View(client);

            }
            catch (Exception)
            {

                return NotFound();
            }
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
                Client client = await _clientHelper.GetClientByIdAsync(id);

                if (client == null)
                {
                    return NotFound();
                }

                City city = await _countryRepository.GetCityAsync(client.User.CityId);
                var country = _countryRepository.GetCountryAsync(city);

                var model = new ClientViewModel
                {
                    Countries = _countryRepository.GetComboCountries(),

                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.User.Email,
                    ClientNumber = client.Client_Number,
                    StreetAddress = client.StreetAddress,
                    PostalCode = client.User.PostalCode,
                    CityId = client.User.CityId,
                    DateofBirth = (DateTime)client.DateofBirth,
                    TaxNumber = client.User.TaxNumber,
                    Identification = client.User.Identification,
                    JoinDate = (DateTime)client.JoinDate,
                    isValidated = client.isClientNumberConfirmed,
                };

                if (country != null)
                {
                    model.CountryId = country.Id;
                    model.Cities = await _countryRepository.GetComboCities(country.Id);
                    model.Countries = _countryRepository.GetComboCountries();
                    model.CityId = client.User.CityId;
                }
                return View(model);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Nunca me fio no que vem da View, fazer sempre o check com a base de dados. ( Fazer a pesquisa por nif, número de identificação e email). QQ um destes campos pode ter mudado, ou pode ter havido um engano
                // Obter o cliente
                Client client = await _clientHelper.GetClientByClientNumberAsync(model.ClientNumber);

                if (client != null)
                {

                    if (client.isClientNumberConfirmed == false && model.isValidated == true) // Administrador aceitou o número de cliente --> Enviar email com a confirmação
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
                var client = _context.Client.Where(x => x.Client_Number == model.ClientNumber).FirstOrDefault();
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.Email = model.Email;
                client.Client_Number = model.ClientNumber;
                client.StreetAddress = model.StreetAddress;
                client.PostalCode = model.PostalCode;
                client.User.CityId = model.CityId;
                client.DateofBirth = (DateTime)model.DateofBirth;
                client.TaxNumber = model.TaxNumber;
                client.Identification = model.Identification;
                client.JoinDate = (DateTime)model.JoinDate;
                client.isClientNumberConfirmed = model.isValidated;

                _context.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
