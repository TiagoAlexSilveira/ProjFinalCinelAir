using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjFinalCinelAirClient.ModelsApi
{
    public class UserViewModelAPI
    {
        public string ClientNumber { get; set; }

        public string MilesStatus { get; set; }

        public string MilesBonus { get; set; }

        public List<Transaction> Transactions { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
    }
}
