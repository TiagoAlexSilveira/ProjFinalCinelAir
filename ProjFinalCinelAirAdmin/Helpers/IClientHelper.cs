using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Helpers
{
    public interface IClientHelper
    {
        List<Client> GetClientsToValidate();

        List<Client> GetClients();

        Task<Client> GetClientByIdAsync(int id);

        Task<Client> GetClientByClientNumberAsync(int clientNumber);

    }
}
