using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class ConvertMilesViewModel : Client
    {
        public int SelectedRadio { set; get; }

        public List<BuyMilesShop> ShopList { get; set; }

        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

    }
}
