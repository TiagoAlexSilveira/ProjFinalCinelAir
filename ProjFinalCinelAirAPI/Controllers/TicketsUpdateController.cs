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
    [Route("[controller]")]
    public class TicketsUpdateController : ControllerBase
    {
        #region
        private readonly ILogger<TicketsUpdateController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        #endregion

        public TicketsUpdateController(ILogger<TicketsUpdateController> logger, DataContext context, IUserHelper userHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            // 1º Data a solicitar à API (dia anterior) - Estamos a correr esta API no dia Seguinte das 1H às 4H
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            string dateString = date.ToString("yyyy-MM-dd"); // Mesmo formato que a base de dados MongoDB


            // 2º Instanciar apiService para aceder à api
            ApiService<List<TicketModel>> apiService = new ApiService<List<TicketModel>>();

            string URL = "https://airlineapinode.azurewebsites.net/";
            string Controller = $"ticket/{dateString}";

            var response = await apiService.GetData(URL, "ticket/2020-10-29"); // Aceder à ApiNode

            List<TicketModel> TicketList = new List<TicketModel>();

            TicketList = (List<TicketModel>)response.Result;

            //3º Percorrer a lista de Bilhetes --> Obter o user de cada bilhete (User Helper...)
            await CinelAirMiles_Users(TicketList);

            return new string[] { "value1", "value2" };
        }


        private async Task CinelAirMiles_Users(List<TicketModel> TicketsList)
        {

            // 1º Verificar se o nif do bilhete corresponde a algum cliente da CinelAir Miles
            foreach (var ticket in TicketsList)
            {
                User client = _userHelper.GetUser(ticket.Client_TaxNumber); // Vai à tabela dos users e devolve o user. No entanto, o user pode não ser cliente
                bool isClient = await _userHelper.isClientAsync(client, "Client"); // Verifica se aquele user tem o role Client

               if (isClient && ticket.StarAlliance) // Cliente existe na base de dados e tem o role Client e o voo foi operado pela CinelAir ou parceiros 
                {

                    DistanceModel model = await GetMiles(ticket); // Aceder à api para obter as milhas percorridas

                    string rate = "TopExecutiva";

                    if (ticket.Class != "TopExecutiva") // Se a viagem tiver sido realizada noutra classe qe não a TopExecutiva
                    {
                        rate = await GetRate(model, ticket.Class); // Aceder à base de dados dos países e obter a taxa
                    }

                    await updateMilesStatus(client, rate, ticket);

                }

            }


        }

        private async Task<DistanceModel> GetMiles(TicketModel ticket)
        {

            // Aceder à api para obter as milhas percorridas
            ApiService<DistanceModel> apiService = new ApiService<DistanceModel>();

            string URL = "https://www.distance24.org/";
            string Controller = $"route.json?stops={ticket.From}|{ticket.To}";

            var response = await apiService.GetData(URL, Controller);

            DistanceModel model = (DistanceModel)response.Result;

            decimal factor = 0.6214M;
            model.distance = model.distance * factor;

            return model;
        }

        private async Task<string> GetRate(DistanceModel model, string classe)
        {
            // É o stopsModel que tem a cena 
            // Aceder à api para obter as milhas percorridas
            ApiService<CountryModel> apiService = new ApiService<CountryModel>();

            string URL = "http://restcountries.eu/";

            CountryModel from = new CountryModel();
            CountryModel to = new CountryModel();
            string rate = "";

            for (int i = 0; i < model.stops.Count; i++)
            {
                string Controller = $"rest/v2/{model.stops[i]}";

                var response = await apiService.GetData(URL, Controller);

                if (i == 0) 
                {
                    from = (CountryModel)response.Result;
                }

                else
                {
                    to = (CountryModel)response.Result;
                }
            }

            // Se a region (continente) do "from" e do "to" forem diferentes, quer dizer que se trata de um voo intercontinental
            if (from.region != to.region)
            {
                rate = $"{classe}_Intercontinentais";
            }

            // Se a subregion de algum dos destinos for Northern Africa ou se a região for Europe

            if (from.subregion == "Northern Africa" && to.subregion == "Northern Africa") // Viagens no norte de África
            {
                rate = $"{classe}_Europa";
            }

            if (from.region == "Europe" && to.region == "Europe") // Viagens na europa
            {
                rate = $"{classe}_Europa";
            }

            if (from.region == "Europe" && to.subregion == "Northern Africa") // Viagens da Europa para o norte de África
            {
                rate = $"{classe}_Europa";
            }

            if (from.subregion == "Northern Africa" && to.region == "Europe") // Viagens do norte de África para a Europa
            {
                rate = $"{classe}_Europa";
            }
            return rate;

        }

        private async Task updateMilesStatus(User user, string rate, TicketModel ticket )
        {

        }
    }
}
