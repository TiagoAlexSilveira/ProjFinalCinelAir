using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Helpers
{
    public interface IConverterHelper
    {

        ChangeUserViewModel ToChangeUserViewModelClient(Client client);

        Client ToClient(ChangeUserViewModel model);
    }
}
