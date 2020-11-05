using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Helpers
{
    public interface IClientHelper
    {
        // Obter o cliente através do seu número de cliente (é o que vem nos bilhetes de avião)
        Client GetClient(int clientNumber);

        // Obter o status do cliente
        Task<string> clientStatusAsync(int clientNumber);
    }


}
