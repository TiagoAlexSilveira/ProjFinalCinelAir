using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public interface IStatusRepository : IGenericRepository<Status>
    {
        Status GetClientStatusByEmail(string email);

        Status GetClientStatusById(int id);
    }
}
