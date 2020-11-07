using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        private readonly DataContext _context;

        public StatusRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public Status GetClientStatusByEmail(string email)
        {
            var client = _context.Client.FirstOrDefault(e => e.Email == email);

            var histStatus = _context.Historic_Status.FirstOrDefault(e => e.ClientId == client.Id);
            var status = _context.Status.FirstOrDefault(e => e.Id == histStatus.StatusId);

            return status;
        }

        public Status GetClientStatusById(int id)
        {
            var histStatus = _context.Historic_Status.FirstOrDefault(e => e.ClientId == id);
            var status = _context.Status.FirstOrDefault(e => e.Id == histStatus.StatusId);

            return status;
        }
    }
}
