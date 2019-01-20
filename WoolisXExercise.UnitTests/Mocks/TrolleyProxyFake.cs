using System;
using WollisXExercise.Models;
using WollisXExercise.Proxies;

namespace WoolisXExercise.UnitTests.Mocks
{
    public class TrolleyProxyFake : ITrolleyProxy
    {
        ProxyResponse<decimal> _response = new ProxyResponse<decimal>();
        ProxyResponse<IEnumerable<CustomerProduct>> _responseCP = new ProxyResponse<IEnumerable<CustomerProduct>>();
        private List<Product> products = new List<Product>()
            {
                new Product(){Name = "Product A", Price= 60.0M, Quantity = 0.0},
                new Product(){Name = "Product B", Price= 600.0M, Quantity = 0.0},
                new Product(){Name = "Product C", Price= 6000.0M, Quantity = 0.0},
                new Product(){Name = "Product D", Price= 60000.0M, Quantity = 0.0},
                new Product(){Name = "Product F", Price= 6000000.0M, Quantity = 0.0}
            };

        public ProductProxyFake()
        {
            _response.Content = products;

            var customerProducts = JsonConvert.DeserializeObject<IEnumerable<CustomerProduct>>("[{\"customerId\": 123, \"products\": [{\"name\": \"Test Product A\",\"price\": 99.99,\"quantity\": 3},{\"name\": \"Test Product B\",\"price\": 101.99,\"quantity\": 1},{\"name\": \"Test Product F\",\"price\": 999999999999,\"quantity\": 1}]},{\"customerId\": 23,\"products\": [{\"name\": \"Test Product A\",\"price\": 99.99,\"quantity\": 2},{\"name\": \"Test Product B\",\"price\": 101.99,\"quantity\": 3},{\"name\": \"Test Product F\",\"price\": 999999999999,\"quantity\": 1}]},{\"customerId\": 23,\"products\": [{\"name\": \"Test Product C\",\"price\": 10.99,\"quantity\": 2},{\"name\": \"Test Product F\",\"price\": 999999999999,\"quantity\": 2}]},{\"customerId\": 23,\"products\": [{\"name\": \"Test Product A\",\"price\": 99.99,\"quantity\": 1},{\"name\": \"Test Product B\",\"price\": 101.99,\"quantity\": 1},{\"name\": \"Test Product C\",\"price\": 10.99,\"quantity\": 1}]}]");
            _responseCP.Content = customerProducts;
        }

        public ProductProxyFake(bool responseHasError)
        {
            if (responseHasError)
            {
                _response.ErrorMessage = "Error";
            }
        }

        public ProductProxyFake(bool responseHasError, bool responseCPHasError)
        {
            if (responseHasError)
            {
                _response.ErrorMessage = "Error";
            }
            else
            {
                _response.Content = products;
            }

            if (responseCPHasError)
            {
                _responseCP.ErrorMessage = "Error";
            }
        }

        public async Task<ProxyResponse<IEnumerable<Product>>> GetProducts()
        {
            return _response;
        }

        public async Task<ProxyResponse<IEnumerable<CustomerProduct>>> GetShopperHistory()
        {
            return _responseCP;
        }
    }
}
