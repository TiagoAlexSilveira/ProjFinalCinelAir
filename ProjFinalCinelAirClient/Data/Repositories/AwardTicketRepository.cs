using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class AwardTicketRepository : GenericRepository<AwardTicket>, IAwardTicketRepository
    {
        public AwardTicketRepository(DataContext context) : base(context)
        {
        }
    }
}
