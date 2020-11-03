using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Helpers
{
    public interface IUserHelper
    {
        // Obter o user através do seu nif (é o que vem nos bilhetes de avião)
        User GetUser(int nif);

        // Obter o user com o role "Client" Associado     
        Task<bool> isClientAsync(User user, string roleName);
    }
}
