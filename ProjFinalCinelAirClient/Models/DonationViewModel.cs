using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class DonationViewModel : Client
    {
        public ICollection<Partner> DonationList { get; set; }

        public int SelectedPartner { get; set; }

        public ICollection<BuyMilesShop> ShopList { get; set; }

        public int SelectedItem { get; set; }
    }
}
