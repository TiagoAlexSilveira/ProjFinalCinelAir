using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Helpers
{
    public class ClientHelper: IClientHelper
    {
        private readonly DataContext _context;

        public ClientHelper(DataContext context)
        {
            _context = context;
        }

        public List<Client> GetClientsToValidate() 
        {
            List<Client> List = _context.Client.Include("User").Where (x=>x.isClientNumberConfirmed == false).ToList();

            return List;
        }

        public List<Client> GetClients() 
        {
            List<Client> List = _context.Client.Include("User").ToList();

            return List;
        }

        public async Task<Client> GetClientByIdAsync(int id) 
        {
            Client client = await _context.Client.Include("User").Where(x=> x.Id == id).FirstOrDefaultAsync();

            return client;
        }


        public async Task<Client> GetClientByClientNumberAsync(int clientNumber)
        {
            Client client = await _context.Client.Include("User").Where(x => x.Client_Number == clientNumber).FirstOrDefaultAsync();

            return client;
        }

       
    }
    
}
