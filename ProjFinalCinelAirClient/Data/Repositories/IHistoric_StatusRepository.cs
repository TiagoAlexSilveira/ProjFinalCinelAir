using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data.Repositories
{
    public interface IHistoric_StatusRepository : IGenericRepository<Historic_Status>
    {
        Historic_Status GetClientHistoric_StatusById(int id);

        Historic_Status GetClientStatusByEmail(string email);
    }
}
