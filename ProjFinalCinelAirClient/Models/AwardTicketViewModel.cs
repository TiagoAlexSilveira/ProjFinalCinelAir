using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class AwardTicketViewModel
    {
        public int Id { get; set; } //client Id

        public string Description { get; set; }

        public int Miles { get; set; }

        public int? Seats { get; set; }

        public int? availableSeats { get; set; }

        public bool isValidated { get; set; }


        public ICollection<AwardTicket> awardsList { get; set; }


    }
}
