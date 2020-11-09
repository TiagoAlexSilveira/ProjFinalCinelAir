using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class Mile_BonusRepository : GenericRepository<Mile_Bonus>, IMile_BonusRepository
    {
        private readonly DataContext _context;

        public Mile_BonusRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
