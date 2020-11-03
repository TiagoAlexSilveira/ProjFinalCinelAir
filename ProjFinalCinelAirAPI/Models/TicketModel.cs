using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Models
{
    public class TicketModel
    {
        public int Id { get; set; }

        public int ticketId { get; set; }

        public int Client_TaxNumber { get; set; }

        public DateTime Date { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool StarAlliance { get; set; }

        public string Class { get; set; }
    }
}
