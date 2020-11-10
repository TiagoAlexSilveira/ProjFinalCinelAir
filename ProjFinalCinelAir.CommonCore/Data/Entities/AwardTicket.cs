using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class AwardTicket: IEntity
    {        
        public int Id { get; set; }

        public string Description { get; set; }

        public int Miles { get; set; }

        public int? Seats { get; set; }

        public int? availableSeats { get; set; }

        public bool? isValidated { get; set; }
    }
}
