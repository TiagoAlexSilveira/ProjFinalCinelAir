using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Travel_Ticket : IEntity
    {
        public int Id { get; set; }

        public int TicketId { get; set; }


        public DateTime Travel_Date { get; set; }

        public string DepartureCity { get; set; }

        public string ArrivalCity { get; set; }


        public int UserId { get; set; }

        public User User { get; set; }

        public int RateId { get; set; }

        public Rate Rate { get; set; }

        public int Miles_Status_Id { get; set; }

        public Mile_Status Miles_Status { get; set; }

        public int Miles_Bonus_Id { get; set; }

        public Mile_Bonus Miles_Bonus { get; set; }

    }
}
