using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Mile_Status : IEntity
    {
        public int Id { get; set; }

        public decimal Miles_Number { get; set; }

        public DateTime Validity { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }


    }
}
