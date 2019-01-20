using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WollisXExercise.Models;

namespace WollisXExercise.Proxies
{
    public interface IProductProxy
    {
        Task<ProxyResponse<IEnumerable<Product>>> GetProducts();

        Task<ProxyResponse<IEnumerable<CustomerProduct>>> GetShopperHistory();
    }

    public class ProductProxy : IProductProxy
    {
        private readonly IConfiguration _configuration;

        public ProductProxy(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ProxyResponse<IEnumerable<Product>>> GetProducts()
        {
            ProxyResponse<IEnumerable<Product>> response = new ProxyResponse<IEnumerable<Product>>();

            try
            {
                var uri = new Uri($"{_configuration[$"{AppSettings.Url}:{AppSettings.Products}"]}?token={_configuration[$"{AppSettings.AuthToken}"]}");

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                var client = new HttpClient();
                var httpResponse = await client.SendAsync(request);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var rawResponse = await httpResponse.Content.ReadAsStringAsync();

                    response.Content = JsonConvert.DeserializeObject<IEnumerable<Product>>(rawResponse);
                }
                else
                {
                    response.ErrorMessage = $"Error response: {httpResponse}";
                }
            }
            catch(Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<ProxyResponse<IEnumerable<CustomerProduct>>> GetShopperHistory()
        {
            ProxyResponse<IEnumerable<CustomerProduct>> response = new ProxyResponse<IEnumerable<CustomerProduct>>();

            try
            {
                var uri = new Uri($"{_configuration[$"{AppSettings.Url}:{AppSettings.ShopperHistory}"]}?token={_configuration[$"{AppSettings.AuthToken}"]}");

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                var client = new HttpClient();
                var httpResponse = await client.SendAsync(request);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var rawResponse = await httpResponse.Content.ReadAsStringAsync();

                    response.Content = JsonConvert.DeserializeObject<IEnumerable<CustomerProduct>>(rawResponse);
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
