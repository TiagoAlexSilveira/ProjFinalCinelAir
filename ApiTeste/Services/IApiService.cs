using ApiTeste.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTeste.Services
{
    public interface IApiService
    {
        Task<Response> GetDataAsync<T>(string urlBase, string controller);
    }
}
