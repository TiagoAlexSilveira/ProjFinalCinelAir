using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Mile_Bonus : IEntity
    {
        public int Id { get; set; }

        public int Miles_Number { get; set; }

        public DateTime Validity { get; set; }

        public string UserId { get; set; }

        public int available_Miles_Bonus { get; set; }

        public User User { get; set; }

    }
}
