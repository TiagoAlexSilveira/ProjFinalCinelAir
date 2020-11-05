using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Collections.Generic;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {

        Client GetClientByUserEmail(string email);

        Client GetClientByUserId(string id);

        IEnumerable<SelectListItem> GetComboClients();

        User GetUserByClientId(int id);

    }
}
