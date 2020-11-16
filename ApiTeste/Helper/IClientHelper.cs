using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTeste.Helper
{
    public interface IClientHelper
    {
        // Obter o cliente através do seu número de cliente (é o que vem nos bilhetes de avião)
        Client GetClient(int clientNumber);

        // Obter o status do cliente
        Task<string> clientStatusAsync(int clientNumber);
    }
}
