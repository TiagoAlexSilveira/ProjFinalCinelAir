using ProjFinalCinelAir.Prism.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Responses
{
    public class UserResponse
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
