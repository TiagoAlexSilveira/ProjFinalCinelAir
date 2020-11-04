using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAPI.Helpers;
using ProjFinalCinelAirAPI.Models;
using ProjFinalCinelAirAPI.Services;


namespace ProjFinalCinelAirAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsUpdateController : ControllerBase
    {
        #region
        private readonly ILogger<TicketsUpdateController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IApiService _apiService;
        #endregion



        public TicketsUpdateController(ILogger<TicketsUpdateController> logger, DataContext context, IUserHelper userHelper, IApiService apiService)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _apiService = apiService;
        }

        [HttpGet]
        public string Get()
        {
            return "Base de dados a ser actualizada";
        }


        [HttpPost]
        public async Task Post() // Novos Registos na Base de dados
        {
            // 1º Data a solicitar à API Node(dia anterior) - Estamos a correr esta API no dia Seguinte das 1H às 4H
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            string dateString = date.ToString("yyyy-MM-dd"); // Mesmo formato que a base de dados MongoDB


            string URL = "https://airlineapinode.azurewebsites.net/";
            string Controller = $"ticket/{dateString}";

            var response = await _apiService.GetDataAsync<List<TicketModel>>(URL, "ticket/2020-10-29"); // Aceder à ApiNode


            List<TicketModel> TicketList = (List<TicketModel>)response.Result;

            //3º Percorrer a lista de Bilhetes --> Obter o user de cada bilhete (User Helper...)
            await CinelAirMiles_Users(TicketList);


        }

        [HttpPut] // Chamado após o Post
        public async Task Put() // Actualizar as milhas do cliente e ver se o estatuto foi alterado com esta viagem
        {
            // 1º Data a solicitar à API Node(dia anterior) - Estamos a correr esta API no dia Seguinte das 1H às 4H
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            string dateString = date.ToString("yyyy-MM-dd"); // Mesmo formato que a base de dados MongoDB


            string URL = "https://airlineapinode.azurewebsites.net/";
            string Controller = $"ticket/{dateString}";

            var response = await _apiService.GetDataAsync<List<TicketModel>>(URL, "ticket/2020-10-29"); // Aceder à ApiNode


            List<TicketModel> TicketList = (List<TicketModel>)response.Result;

            

            foreach (var ticket in TicketList) // Percorrer a lista de bilhetes
            {

                User client = _userHelper.GetUser(ticket.ClientNumber);
                
                UpadateMilesStatus(client); // Actualizar as milhas_Status
               
                UpdateMilesBonus(client);   // Actualizar as milhas_Bonus         

                UpdateStatus(client); // Actualizar o Status do cliente
            }
        }

        private async Task CinelAirMiles_Users(List<TicketModel> TicketsList)
        {
            foreach (var ticket in TicketsList)
            {

                User client = _userHelper.GetUser(ticket.ClientNumber); // Vai à tabela obter o user que tem o numero de cliente do bilhete         

                if (client != null) // Cliente existe
                {

                    DistanceModel model = await GetMiles(ticket); // Aceder à api para obter as milhas percorridas

                    string rate = "TopExecutiva";

                    if (ticket.Class != "TopExecutiva") // Se a viagem tiver sido realizada noutra classe qe não a TopExecutiva
                    {
                        rate = await GetRate(model, ticket.Class); // Aceder à base de dados dos países e obter a taxa
                    }

                    await NewEntries(client, rate, ticket, model);

                }
            }
        }

        private async Task<DistanceModel> GetMiles(TicketModel ticket)
        {

            // Aceder à api para obter as milhas percorridas
            string URL = "https://www.distance24.org/";
            string Controller = $"route.json?stops={ticket.From}|{ticket.To}";

            var response = await _apiService.GetDataAsync<DistanceModel>(URL, Controller); // Aceder à ApiNode
            DistanceModel model = (DistanceModel)response.Result;

            decimal factor = 0.6214M;
            model.distance = model.distance * factor;

            return model;
        }

        private async Task<string> GetRate(DistanceModel model, string classe)
        {
            // É o stopsModel que tem a cena 
            // Aceder à api para obter as milhas percorridas
            string URL = "http://restcountries.eu/";

            CountryModel from = new CountryModel();
            CountryModel to = new CountryModel();
            string rate = "";

            for (int i = 0; i < model.stops.Count; i++)
            {
                string Controller = $"rest/v2/alpha/{model.stops[i].countryCode}";

                var response = await _apiService.GetDataAsync<CountryModel>(URL, Controller); // Aceder à ApiNode

                if (i == 0)
                {
                    from = (CountryModel)response.Result;
                }

                else
                {
                    to = (CountryModel)response.Result;
                }
            }

            // Se a subregion de algum dos destinos for Northern Africa ou se a região for Europe

            if (from.subregion == "Northern Africa" && to.subregion == "Northern Africa") // Viagens no norte de África
            {
                rate = $"{classe}_Europa";
            }

            else if (from.region == "Europe" && to.region == "Europe") // Viagens na europa
            {
                rate = $"{classe}_Europa";
            }

            else if (from.region == "Europe" && to.subregion == "Northern Africa") // Viagens da Europa para o norte de África
            {
                rate = $"{classe}_Europa";
            }

            else if (from.subregion == "Northern Africa" && to.region == "Europe") // Viagens do norte de África para a Europa
            {
                rate = $"{classe}_Europa";
            }

            else
            {
                rate = $"{classe}_Intercontinentais";

            }
            return rate;

        }

        private async Task NewEntries(User user, string rateName, TicketModel ticket, DistanceModel milesTrip)
        {
            int miles = await CalculateMiles(user, rateName, milesTrip);

            // Cálculo das milhas --> Necessário para a inserção do bilhete
            Rate rate = _context.Rate
                        .Where(x => x.Description == rateName).FirstOrDefault();

            DateTime date = new DateTime(DateTime.Now.Year + 3, DateTime.Now.Month, DateTime.Now.Day); // Milhas Válidas por 3 anos


            // ================= Realizar as entradas nas tabelas Miles_Status e Miles_Bonus
            if (ticket.StarAlliance) // Se a viagem foi realizada pela StarAlliance --> as milhas vão para milhas Status
            {
                _context.Mile_Status.Add(new Mile_Status
                {
                    Miles_Number = miles,
                    available_Miles_Status = miles,
                    UserId = user.Id,
                    Validity = date
                });
            }

            else // A viagem foi realizada por companhias aéreas parceiras da CinelAir --> as milhas vão para milhas Bonus
            {
                _context.Mile_Bonus.Add(new Mile_Bonus
                {
                    Miles_Number = miles,
                    available_Miles_Bonus = miles,
                    UserId = user.Id,
                    Validity = date
                });
            }

            _context.SaveChanges(); // Registar as entradas na base de dados

            // ================= Realizar a entrada na tabela de Travel_Tickets====================

            if (ticket.StarAlliance) // Se a viagem foi realizada pela StarAlliance --> Bilhete tem milhas Status associadas
            {
                // Ir à tabela de milhas status obter o id da última entrada
                var lastEntrie = _context.Mile_Status.OrderByDescending(u => u.Id).FirstOrDefault();

                // Inserir o bilhete na tabela dos Travel_Ticket
                _context.Travel_Ticket.Add(new Travel_Ticket
                {
                    TicketId = ticket.Id,
                    Travel_Date = ticket.Date,
                    DepartureCity = ticket.From,
                    ArrivalCity = ticket.To,
                    UserId = user.Id,
                    RateId = rate.Id,
                    Miles_StatusId = lastEntrie.Id,
                });
            }

            else // A viagem foi realizada por companhias aéreas parceiras da CinelAir --> Bilhete tem milhas Bonus associadas
            {
                // Ir à tabela de milhas bonnus obter o id da última entrada
                var lastEntrie = _context.Mile_Bonus.OrderByDescending(u => u.Id).FirstOrDefault();

                // Inserir o bilhete na tabela dos Travel_Ticket
                _context.Travel_Ticket.Add(new Travel_Ticket
                {
                    TicketId = ticket.Id,
                    Travel_Date = ticket.Date,
                    DepartureCity = ticket.From,
                    ArrivalCity = ticket.To,
                    UserId = user.Id,
                    RateId = rate.Id,
                    Miles_BonusId = lastEntrie.Id,
                });
            }          
            _context.SaveChanges(); // Guardar a entrada do bilhete na tabela de Travel_Ticket

        }

        

        private async Task<int> CalculateMiles(User user, string rateName, DistanceModel milesTrip) 
        {

            // Saber qual o status do cliente
            string status = await _userHelper.clientStatusAsync(user.Id);

            // Cálculo das milhas 
            Rate rate = _context.Rate
                        .Where(x => x.Description == rateName).FirstOrDefault();

            int milesFlown = Convert.ToInt32(Convert.ToDecimal(rate.Percentage)/100 * milesTrip.distance); // Bonificação relativa ao tipo de bilhete
            int miles = milesFlown;

            if (status == "Silver") // Se o cliente é Silver tem uma bonificação de 25% face ao total de milhas voadas
            {
                miles += Convert.ToInt32(0.25M * milesTrip.distance);

            }

            if (status == "Gold") // Se o cliente é Gold tem uma bonificação de 50% face ao total de milhas voadas
            {
                miles += Convert.ToInt32(0.5M * milesTrip.distance);

            }

            return miles;


        }

        private void UpadateMilesStatus(User user) 
        {
            // Ir à tabela de milhas status e contabilizar as milhas disponiveis

           var list = _context.Mile_Status.Where(x=> x.Validity >= DateTime.Now && x.available_Miles_Status>0).ToList();

            int miles_status = 0;

            foreach (var item in list)
            {
                miles_status += item.available_Miles_Status;

            }

            // Realizar a actualização das milhas no cliente
            var clientQuery = (from client in _context.Users where client.Id == user.Id select client).ToList(); // Obtenho o cliente

            clientQuery.ForEach(x => x.Miles_Status = miles_status); // Actualizar o campo das milhas status

            _context.SaveChanges();


        }

        private void UpdateMilesBonus(User user)
        {

            // Ir à tabela de milhas status e contabilizar as milhas disponiveis

            var list = _context.Mile_Bonus.Where(x => x.Validity >= DateTime.Now && x.available_Miles_Bonus > 0).ToList();

            int miles_status = 0;

            foreach (var item in list)
            {
                miles_status += item.available_Miles_Bonus;

            }

            // Realizar a actualização das milhas no cliente
            var clientQuery = (from client in _context.Users where client.Id == user.Id select client).ToList(); // Obtenho o cliente

            clientQuery.ForEach(x => x.Miles_Status = miles_status); // Actualizar o campo das milhas status

            _context.SaveChanges();
        }

        private void UpdateStatus(User user) 
        {

            // Verificar o status do cliente e ver se as milhas são suficientes para alterar o status
            // Realizar a actualização das milhas no cliente
            DateTime date = new DateTime(DateTime.Now.Year -1, DateTime.Now.Month, DateTime.Now.Day); // Milhas obtidas no último ano
            var ticketQuery = (from ticket in _context.Travel_Ticket where ticket.UserId == user.Id && ticket.Travel_Date >= date select ticket).ToList(); // Obtenho o cliente

            int miles_status_sum = 0;
            int voos = 0;

            foreach (var item in ticketQuery)
            {
                // Obter as milhas status correspondentes ao voo
                var miles_status = (from miles in _context.Mile_Status where miles.Id == item.Miles_StatusId select miles).FirstOrDefault(); // Obtenho o cliente

                miles_status_sum += miles_status.Miles_Number; // Somar as milhas

                voos += 1;
            }

            var status = (from userStatus in _context.Historic_Status where userStatus.UserId == user.Id && userStatus.End_Date == null select userStatus).FirstOrDefault(); // Obtenho o cliente

            // 1- Basic; 2 - Silver; 3 - Gold

            if ((miles_status_sum >= 30000 || voos>= 25) && status.StatusId == 1 ) // Critérios para cliente mudar de status para Silver (apenas no caso de ser básico) 
            {
                // Realizar a actualização do status do cliente no histórico
                var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.UserId == user.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault(); 


                clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                _context.Historic_Status.Add(new Historic_Status
                {
                    Start_Date = DateTime.Now,
                    isValidated = false,
                    StatusId = 2,
                    UserId = user.Id
                });

                _context.SaveChanges();


            }

            else if ((miles_status_sum >= 70000 || voos >= 50) && (status.StatusId == 1 || status.StatusId == 2)) // Critérios para cliente mudar de status para Gold(no caso de ser básico ou Silver) 
            {
                // Realizar a actualização do status do cliente no histórico
                var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.UserId == user.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault();


                clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                _context.Historic_Status.Add(new Historic_Status
                {
                    Start_Date = DateTime.Now,
                    isValidated = false,
                    StatusId = 3,
                    UserId = user.Id
                });

                _context.SaveChanges();

            }
        }
    }
}
