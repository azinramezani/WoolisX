using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WollisXExercise.Models;
using WollisXExercise.Proxies;


namespace WollisXExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController: ControllerBase
    {
        private readonly IProductProxy _productProxy;

        public ProductController(IProductProxy productProxy)
        {
            _productProxy = productProxy;
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Product>>> Sort([FromQuery] string sortOption)
        {
            if(string.IsNullOrEmpty(sortOption))
            {
                return BadRequest("sortOption is missing from query string!");
            }

            var getProductsResult = await _productProxy.GetProducts();

            if (!string.IsNullOrEmpty(getProductsResult.ErrorMessage))
            {
                return BadRequest(getProductsResult.ErrorMessage);
            }

            var products = getProductsResult.Content;

            if (SortOptions.Low.ToString().Equals(sortOption))
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (SortOptions.High.ToString().Equals(sortOption))
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else if (SortOptions.Ascending.ToString().Equals(sortOption))
            {
                products = products.OrderBy(p => p.Name);
            }
            else if (SortOptions.Descending.ToString().Equals(sortOption))
            {
                products = products.OrderByDescending(p => p.Name);
            }
            else if (SortOptions.Recommended.ToString().Equals(sortOption))
            {
                var shopperHistoryResult = await _productProxy.GetShopperHistory();
                if (!string.IsNullOrEmpty(shopperHistoryResult.ErrorMessage))
                {
                    return BadRequest(shopperHistoryResult.ErrorMessage);
                }


                var productCounts = shopperHistoryResult.Content
                .SelectMany(sh => sh.Products)
                    .GroupBy(p => p.Name)
                    .Select(prod => new { Name = prod.Key, Count = prod.Sum(p => p.Quantity) });

                var result = products
                    .GroupJoin(productCounts, product => product.Name, productCount => productCount.Name, (x, y) => new { products = x, productCounts = y })
                    .SelectMany(x => x.productCounts.DefaultIfEmpty(),
                    (x, y) => new Product
                    {
                        Name = x.products.Name,
                        Price = x.products.Price,
                        Quantity = y?.Count ?? 0
                    });

                products = result.OrderByDescending(p => p.Quantity);
            }

            return Ok(products);
        }
    }
}
