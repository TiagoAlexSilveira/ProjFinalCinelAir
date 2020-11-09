using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class TransferMilesViewModel : Client
    {
        public ICollection<Client> ClientList { get; set; }

        public List<BuyMilesShop> ShopList { get; set; }

        public string Message { get; set; }

        public int SelectedRadio { set; get; }

        public string SelectedClientNumber { set; get; }

    }
}
