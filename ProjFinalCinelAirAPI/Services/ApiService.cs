using Newtonsoft.Json;
using ProjFinalCinelAirAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response> GetDataAsync<T>(string urlBase, string controller)
        {
            // Tudo que envolve comunicação com API´s deverá estar asssegurado com um try... catch

            try
            {
                // 1º Criar uma ligação de internet
                var client = new HttpClient();

                // 2º Passar o endereço da API
                client.BaseAddress = new Uri(urlBase);

                // 3º Guardar a resposta do controlador numa variavel
                var response = await client.GetAsync(controller);

                // 4º Guardar a resposta numa variável
                var result = await response.Content.ReadAsStringAsync();

                // Se tiver corrido algum erro, vamos enviar para fora um objecto do tipo Response com a propriedade IsSuccess igual a false e a Message com o valor do resultado
                if (!response.IsSuccessStatusCode)
                {
                    T resultadoRespostaApi = JsonConvert.DeserializeObject<T>(result);

                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                        Result = resultadoRespostaApi

                    };
                }

                // Se tiver corrido tudo bem vamos enviar para fora um objecto do tipo Response com os bilhetes
                else
                {
                    T resultadoRespostaApi = JsonConvert.DeserializeObject<T>(result);

                    return new Response
                    {
                        IsSuccess = true,
                        Result = resultadoRespostaApi
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
