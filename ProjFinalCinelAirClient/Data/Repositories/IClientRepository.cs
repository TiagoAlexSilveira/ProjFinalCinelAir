﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Collections.Generic;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        void ConvertMiles(int milesToPay, Client client, List<Mile_Bonus> list);
        List<BuyMilesShop> ConvertMilesAmountSelection(string status, Client client, List<BuyMilesShop> shopList);
        int DeductMilesWitCut(int milesToPay, Client client, List<Mile_Bonus> list);
        void DeductMilesWithoutCut(int milesToPay, List<Mile_Bonus> list);
        List<Client> GetAllClientsWithStatusBasicOrSilver();
        Client GetClientByClientNumber(int number);
        Client GetClientByUserEmail(string email);

        Client GetClientByUserId(string id);

        IEnumerable<SelectListItem> GetComboClients();

        User GetUserByClientId(int id);

    }
}
