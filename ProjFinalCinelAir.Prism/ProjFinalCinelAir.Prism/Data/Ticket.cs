using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Data
{
    public class Ticket
    {
        public int Id { get; set; }

        public string ticketId { get; set; }

        public int ClientNumber { get; set; }

        public DateTime Date { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool StarAlliance { get; set; }

        public string Class { get; set; }
    }
}
