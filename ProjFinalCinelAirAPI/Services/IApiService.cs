using ProjFinalCinelAirAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Services
{
    public interface IApiService
    {
        Task<Response> GetDataAsync<T>(string urlBase, string controller);
    }
}
