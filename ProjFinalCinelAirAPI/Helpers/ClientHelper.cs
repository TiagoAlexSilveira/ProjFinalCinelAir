using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Helpers
{
    public class ClientHelper : IClientHelper
    {
        #region atributos
        private readonly DataContext _context;

        #endregion



        public ClientHelper(DataContext context)
        {           
            _context = context;
        }


        public async Task<string> clientStatusAsync(int clientId)
        {
            var clientStatus = await _context.Historic_Status
                            .Include(x => x.Status)
                            .Where(x => x.Client.Id == clientId && x.End_Date == null)
                            .FirstOrDefaultAsync();

            return clientStatus.Status.Description;
        }



        public Client GetClient(int clientNumber)
        {
            Client client = _context.Client.Where(x => x.Client_Number == clientNumber).FirstOrDefault();
            return client;
        }
    }
}
