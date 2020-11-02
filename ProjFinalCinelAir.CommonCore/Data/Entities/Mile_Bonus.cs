using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Mile_Bonus : IEntity
    {
        public int Id { get; set; }

        public decimal Miles_Number { get; set; }

        public DateTime Validity { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
