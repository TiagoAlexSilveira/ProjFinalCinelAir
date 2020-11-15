using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class PaymentViewModel
    {
        public int ClientId { get; set; }

        public string Movement_Type { get; set; }

        public int MileQuantity { get; set; }

        public int Price { get; set; }

        public int CardHolderName { get; set; }
        public int CardNumber { get; set; }
        public int ExpirationDate { get; set; }

        //////só para transfer
        public string selectedClientFullName { get; set; }

        public int selectedClientNumber { get; set; }
    }
}
