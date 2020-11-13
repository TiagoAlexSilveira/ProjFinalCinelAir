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
        #region atributos
        private readonly ILogger<TicketsUpdateController> _logger;
        private readonly DataContext _context;
        private readonly IClientHelper _clientHelper;
        private readonly IApiService _apiService;
        #endregion



        public TicketsUpdateController(ILogger<TicketsUpdateController> logger, DataContext context, IClientHelper clientHelper, IApiService apiService)
        {
            _logger = logger;
            _context = context;
            _clientHelper = clientHelper;
            _apiService = apiService;
        }

        [HttpGet]
        public string Get()
        {
            return "Base de dados a ser actualizadaaaaaa.";
        }


        [HttpPost]
        public async Task Post() // Novos Registos na Base de dados
        {
            // 1º Data a solicitar à API Node(dia anterior) - Estamos a correr esta API no dia Seguinte das 1H às 4H
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            string dateString = date.ToString("yyyy-MM-dd"); // Mesmo formato que a base de dados MongoDB


            string URL = "https://airlineapinode.azurewebsites.net/";
            string Controller = $"ticket/{dateString}";

            var response = await _apiService.GetDataAsync<List<TicketModel>>(URL, dateString); // Aceder à ApiNode


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

            var response = await _apiService.GetDataAsync<List<TicketModel>>(URL, dateString); // Aceder à ApiNode


            List<TicketModel> TicketList = (List<TicketModel>)response.Result;

            if (TicketList!=null)
            {

                foreach (var ticket in TicketList) // Percorrer a lista de bilhetes
                {

                    Client client = _clientHelper.GetClient(ticket.ClientNumber);

                    UpadateMilesStatus(client); // Actualizar as milhas_Status

                    UpdateMilesBonus(client);   // Actualizar as milhas_Bonus         

                    UpdateStatus(client); // Actualizar o Status do cliente

                    UpdateAllStatus();

                }
            }

           
        }

        private async Task CinelAirMiles_Users(List<TicketModel> TicketsList)
        {
            try
            {
                if (TicketsList!=null)
                {

                    foreach (var ticket in TicketsList)
                    {

                        Client client = _clientHelper.GetClient(ticket.ClientNumber); // Vai à tabela obter o user que tem o numero de cliente do bilhete         

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

            }
            catch (Exception)
            {

                throw;
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

        private async Task NewEntries(Client client, string rateName, TicketModel ticket, DistanceModel milesTrip)
        {
            int miles = await CalculateMiles(client, rateName, milesTrip);

            // Cálculo das milhas --> Necessário para a inserção do bilhete
            Rate rate = _context.Rate
                        .Where(x => x.Description == rateName).FirstOrDefault();

            DateTime date = new DateTime(DateTime.Now.Year + 3, DateTime.Now.Month, DateTime.Now.Day); // Milhas Válidas por 3 anos


            DateTime dateTransaction = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            // ================= Realizar as entradas nas tabelas Miles_Status e Miles_Bonus e Tabela de Transações
            if (ticket.StarAlliance) // Se a viagem foi realizada pela StarAlliance --> as milhas vão para milhas Status
            {
                _context.Transaction.Add(new Transaction
                {
                    ClientId = client.Id,
                    Movement_Type = "Flight",
                    Date = dateTransaction,
                    Description = $"Flight from {ticket.From} to {ticket.To}",
                    Miles = miles,
                    Balance_Miles_Status = client.Miles_Status + miles,
                    Balance_Miles_Bonus = client.Miles_Bonus
                });

                _context.Mile_Status.Add(new Mile_Status
                {
                    Miles_Number = miles,
                    available_Miles_Status = miles,
                    ClientId = client.Id,
                    Validity = date
                });

              
            }

            else // A viagem foi realizada por companhias aéreas parceiras da CinelAir --> as milhas vão para milhas Bonus
            {
                _context.Transaction.Add(new Transaction
                {
                    ClientId = client.Id,
                    Movement_Type = "Flight",
                    Date = dateTransaction,
                    Description = $"Flight from {ticket.From} to {ticket.To}",
                    Miles = miles,
                    Balance_Miles_Status = client.Miles_Status,
                    Balance_Miles_Bonus = client.Miles_Bonus + miles
                });

                _context.Mile_Bonus.Add(new Mile_Bonus
                {
                    Miles_Number = miles,
                    available_Miles_Bonus = miles,
                    ClientId = client.Id,
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
                    TicketId = Convert.ToInt32(ticket.ticketId),
                    Travel_Date = ticket.Date,
                    DepartureCity = ticket.From,
                    ArrivalCity = ticket.To,
                    ClientId = client.Id,
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
                    TicketId = Convert.ToInt32(ticket.ticketId),
                    Travel_Date = ticket.Date,
                    DepartureCity = ticket.From,
                    ArrivalCity = ticket.To,
                    ClientId = client.Id,
                    RateId = rate.Id,
                    Miles_BonusId = lastEntrie.Id,
                });
            }          
            _context.SaveChanges(); // Guardar a entrada do bilhete na tabela de Travel_Ticket


           
        }

        

        private async Task<int> CalculateMiles(Client client, string rateName, DistanceModel milesTrip) 
        {

            // Saber qual o status do cliente
            string status = await _clientHelper.clientStatusAsync(client.Id);

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

        private void UpadateMilesStatus(Client client) 
        {
            // Ir à tabela de milhas status e contabilizar as milhas disponiveis

           var list = _context.Mile_Status.Where(x=> x.Validity >= DateTime.Now && x.available_Miles_Status>0 && x.ClientId == client.Id).ToList();

            int miles_status = 0;

            foreach (var item in list)
            {
                miles_status += item.available_Miles_Status;

            }

            // Realizar a actualização das milhas no cliente
            var clientQuery = (from inputClient in _context.Client where inputClient.Id == client.Id select inputClient).ToList(); // Obtenho o cliente

            clientQuery.ForEach(x => x.Miles_Status = miles_status); // Actualizar o campo das milhas status

            _context.SaveChanges();


        }

        private void UpdateMilesBonus(Client client) // Actualizar o campo que está como propriedade no cliente
        {

            // Ir à tabela de milhas status e contabilizar as milhas disponiveis

            var list = _context.Mile_Bonus.Where(x => x.Validity >= DateTime.Now && x.available_Miles_Bonus > 0 && x.ClientId == client.Id).ToList();

            int miles_bonus = 0;

            foreach (var item in list)
            {
                miles_bonus += item.available_Miles_Bonus;

            }

            // Realizar a actualização das milhas no cliente
            var clientQuery = (from inputClient in _context.Client where inputClient.Id == client.Id select inputClient).ToList(); // Obtenho o cliente

            clientQuery.ForEach(x => x.Miles_Bonus = miles_bonus); // Actualizar o campo das milhas status

            _context.SaveChanges();
        }

        private void UpdateStatus(Client client)
        {
            
            // Verificar o status do cliente e ver se as milhas são suficientes para alterar o status
            // Realizar a actualização das milhas no cliente
           
            DateTime date = new DateTime(DateTime.Now.Year -1, client.JoinDate.Value.Month, client.JoinDate.Value.Day); // Milhas obtidas no último ano (mas a data de activação)
            var ticketQuery = (from ticket in _context.Travel_Ticket where ticket.ClientId == client.Id && ticket.Travel_Date >= date select ticket).ToList(); // Obtenho os voos realizados no último ano

            int miles_status_sum = 0;
            int voos = 0;

            foreach (var item in ticketQuery)
            {
                // Obter as milhas status correspondentes ao voo
                var miles_status = (from miles in _context.Mile_Status where miles.Id == item.Miles_StatusId select miles).FirstOrDefault(); // Obtenho o cliente

                miles_status_sum += miles_status.Miles_Number; // Somar as milhas

                voos += 1;
            }

            var status = (from clientStatus in _context.Historic_Status where clientStatus.ClientId == client.Id && clientStatus.End_Date == null select clientStatus).FirstOrDefault(); // Obtenho o cliente

            // 1- Basic; 2 - Silver; 3 - Gold =======> Legenda dos Status

            if ((miles_status_sum >= 30000 || voos>= 25) && status.StatusId == 1 ) // Critérios para cliente mudar de status para Silver (apenas no caso de ser básico) 
            {
                // Realizar a actualização do status do cliente no histórico
                var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.ClientId == client.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault(); 


                clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                _context.Historic_Status.Add(new Historic_Status
                {
                    Start_Date = DateTime.Now,
                    isValidated = false,
                    StatusId = 2,
                    ClientId = client.Id
                });

                _context.SaveChanges();


            }

            else if ((miles_status_sum >= 70000 || voos >= 50) && (status.StatusId == 1 || status.StatusId == 2)) // Critérios para cliente mudar de status para Gold(no caso de ser básico ou Silver) 
            {
                // Realizar a actualização do status do cliente no histórico
                var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.ClientId == client.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault();


                clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                _context.Historic_Status.Add(new Historic_Status
                {
                    Start_Date = DateTime.Now,
                    isValidated = false,
                    StatusId = 3,
                    ClientId = client.Id
                });

                _context.SaveChanges();

            }
        }

        private void UpdateAllStatus() 
        {
            // Obter a lista de utilizadores que tiveram activação neste dia do mês
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var listClients = _context.Client.Where(x => x.JoinDate.Value.Month == date.Month && x.JoinDate.Value.Day == x.JoinDate.Value.Day);

            // Actualizar cada cliente
            foreach (var client in listClients)
            {

                DateTime lastYear = new DateTime(DateTime.Now.Year-1, DateTime.Now.Month, DateTime.Now.Day);

                var ticketList = (from ticket in _context.Travel_Ticket where ticket.ClientId == client.Id && ticket.Travel_Date >= lastYear select ticket).ToList(); // Obtenho os voos realizados no último ano

                int miles_status_sum = 0;
                int voos = 0;

                foreach (var item in ticketList)
                {
                    // Obter as milhas status correspondentes ao voo
                    var miles_status = (from miles in _context.Mile_Status where miles.Id == item.Miles_StatusId select miles).FirstOrDefault(); // Obtenho o cliente

                    miles_status_sum += miles_status.Miles_Number; // Somar as milhas

                    voos += 1;
                }

                var status = (from clientStatus in _context.Historic_Status where clientStatus.ClientId == client.Id && clientStatus.End_Date == null select clientStatus).FirstOrDefault(); // Obtenho o cliente

                // 1- Basic; 2 - Silver; 3 - Gold =======> Legenda dos Status

                if ((miles_status_sum >= 30000 || voos >= 25) && status.StatusId == 1) // Critérios para cliente mudar de status para Silver (apenas no caso de ser básico) 
                {
                    // Realizar a actualização do status do cliente no histórico
                    var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.ClientId == client.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault();


                    clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                    _context.Historic_Status.Add(new Historic_Status
                    {
                        Start_Date = DateTime.Now,
                        isValidated = false,
                        StatusId = 2,
                        ClientId = client.Id
                    });

                    _context.SaveChanges();


                }

                else if ((miles_status_sum >= 70000 || voos >= 50) && (status.StatusId == 1 || status.StatusId == 2)) // Critérios para cliente mudar de status para Gold(no caso de ser básico ou Silver) 
                {
                    // Realizar a actualização do status do cliente no histórico
                    var clientQuery = (from historicStatus in _context.Historic_Status where historicStatus.ClientId == client.Id && historicStatus.End_Date == null select historicStatus).FirstOrDefault();


                    clientQuery.End_Date = DateTime.Now; // Actualizar o campo das milhas status


                    _context.Historic_Status.Add(new Historic_Status
                    {
                        Start_Date = DateTime.Now,
                        isValidated = false,
                        StatusId = 3,
                        ClientId = client.Id
                    });

                    _context.SaveChanges();

                }

            }

     
        }

      
    }
}
