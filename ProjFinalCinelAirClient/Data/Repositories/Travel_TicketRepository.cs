using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class Travel_TicketRepository : GenericRepository<Travel_Ticket>, ITravel_TicketRepository
    {
        public Travel_TicketRepository(DataContext context) : base(context)
        {
        }
    }
}
