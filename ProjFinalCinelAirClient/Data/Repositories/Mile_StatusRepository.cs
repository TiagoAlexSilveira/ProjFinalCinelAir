﻿using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class Mile_StatusRepository : GenericRepository<Mile_Status>, IMile_StatusRepository
    {
        public Mile_StatusRepository(DataContext context) : base(context)
        {
        }
    }
}
