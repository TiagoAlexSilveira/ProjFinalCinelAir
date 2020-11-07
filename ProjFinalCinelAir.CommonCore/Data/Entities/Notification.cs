using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public string Subject  { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

    }
}
