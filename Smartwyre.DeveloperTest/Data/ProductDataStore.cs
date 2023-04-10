using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    private readonly List<Product> _products;

    public ProductDataStore()
    {
        _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Identifier = "Mangoes",
                Price = 100M,
                Uom = "lbs",
                SupportedIncentives = (SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.FixedCashAmount),
            },
            new Product
            {
                Id = 2,
                Identifier = "Bananas",
                Price = 200M,
                Uom = "lbs",
                SupportedIncentives = (SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom | SupportedIncentiveType.FixedRateRebate),
            },
            new Product
            {
                Id = 3,
                Identifier = "Rice",
                Price = 300M,
                Uom = "Kgs",
                SupportedIncentives = (SupportedIncentiveType.AmountPerUom),
            },
            new Product
            {
                Id = 4,
                Identifier = "Sugarcane",
                Price = 400M,
                Uom = "lbs",
                SupportedIncentives = (SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom),
            },
        };
    }


    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return _products.FirstOrDefault(x => x.Identifier == productIdentifier);
    }
}
