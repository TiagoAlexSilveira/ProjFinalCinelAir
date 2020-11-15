using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class BuyMilesViewModel : Client
    {
       
        public int Miles_Number { get; set; }

        public DateTime Validity { get; set; }

        public int available_Miles_Bonus { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }


        public ICollection<BuyMilesShop> ShopList { get; set; }

        public int SelectedRadio { set; get; }

        //payment
        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }

    }
}
