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
        public BuyMilesShopRepository(DataContext context) : base(context)
        {
        }
    }
}
