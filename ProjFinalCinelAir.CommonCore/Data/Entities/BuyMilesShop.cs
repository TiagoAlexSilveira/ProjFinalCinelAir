using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class BuyMilesShop :IEntity
    {
        public int Id { get; set; }

        public int MileQuantity { get; set; }

        public decimal Price { get; set; }

    }
}
