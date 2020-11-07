using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class Historic_StatusRepository : GenericRepository<Historic_Status>, IHistoric_StatusRepository
    {
        private readonly DataContext _context;

        public Historic_StatusRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public Historic_Status GetClientHistoric_StatusByEmail(string email)
        {
            var client = _context.Client.FirstOrDefault(e => e.Email == email);

            var status = _context.Historic_Status.FirstOrDefault(e => e.ClientId == client.Id);

            return status;
        }


        public Historic_Status GetClientHistoric_StatusById(int id)
        {
            var status = _context.Historic_Status.FirstOrDefault(e => e.ClientId == id);

            return status;
        }

 
    }
}
