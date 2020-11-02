using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Rate : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Percentage { get; set; }


    }
}
