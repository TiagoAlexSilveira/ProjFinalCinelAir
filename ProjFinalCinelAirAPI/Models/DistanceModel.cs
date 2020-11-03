using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Models
{
    public class DistanceModel
    {
        public decimal distance { get; set; }

        public List<StopsModel> stops { get; set; }
    }
}
