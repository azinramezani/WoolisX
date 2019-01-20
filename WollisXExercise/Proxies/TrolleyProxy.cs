using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WollisXExercise.Models;

namespace WollisXExercise.Proxies
{
    public interface ITrolleyProxy
    {
        Task<ProxyResponse<decimal>> CalculateTrolley(TrolleyCalculatorRequest request);
    }

    public class TrolleyProxy : ITrolleyProxy
    {
        private readonly IConfiguration _configuration;

        public TrolleyProxy(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ProxyResponse<decimal>> CalculateTrolley(TrolleyCalculatorRequest request)
        {
            ProxyResponse<decimal> response = new ProxyResponse<decimal>();
            if (request == null)
            {
                response.ErrorMessage = "Request is null!";
                return response;
            }

            try
            {
                var uri = new Uri($"{_configuration[$"{AppSettings.Url}:{AppSettings.TrolleyCalculator}"]}?token={_configuration[$"{AppSettings.AuthToken}"]}");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                };


              var client = new HttpClient();
            
              var httpResponse = await client.SendAsync(httpRequest);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var rawResponse = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<decimal>(rawResponse);

                    response.Content = result;
                }
                else
                {
                    response.ErrorMessage = $"Error response: {httpResponse}";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

    }
}
