using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        public CardRepository(DataContext context) : base(context)
        {
        }
    }
}
