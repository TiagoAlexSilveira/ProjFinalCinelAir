using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class ConvertMilesViewModel : Client
    {
        public int SelectedRadio { set; get; }

        public List<BuyMilesShop> ShopList { get; set; }

    }
}
