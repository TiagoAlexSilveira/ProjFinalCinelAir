using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjFinalCinelAir.CommonCore.Responses;

namespace ProjFinalCinelAir.CommonCore.Services
{
    public interface IApiService
    {

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
