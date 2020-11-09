using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class BuyMilesShopRepository : GenericRepository<BuyMilesShop>, IBuyMilesShopRepository
    {
        private readonly DataContext _context;

        public BuyMilesShopRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
