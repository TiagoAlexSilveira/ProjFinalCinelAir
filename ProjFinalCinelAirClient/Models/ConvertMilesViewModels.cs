using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class ConvertMilesViewModels
    {
        public int SelectedAmount { get; set; }

        public List<BuyMilesShop> ShopList { get; set; }

    }
}
