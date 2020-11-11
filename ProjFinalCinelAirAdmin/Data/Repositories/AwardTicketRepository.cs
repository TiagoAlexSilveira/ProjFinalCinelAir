﻿using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Data.Repositories
{
    public class AwardTicketRepository : GenericRepository<AwardTicket>, IAwardTicketRepository
    {

        private readonly DataContext _context;

        public AwardTicketRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
    
}
