using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private readonly List<Rebate> _rebates;

    public RebateDataStore()
    {
        _rebates = new List<Rebate>
        {
            new Rebate
            {
                Identifier = "AmountPerUom",
                Incentive = IncentiveType.AmountPerUom,
                Amount = 1000m,
                Percentage = 0,
            },
            new Rebate
            {
                Identifier = "FixedCashAmount",
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 2000m,
                Percentage = 0,
            },
            new Rebate
            {
                Identifier = "FixedRateRebate",
                Incentive = IncentiveType.FixedRateRebate,
                Amount = 3000m,
                Percentage = 10,
            }
        };
    }

    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return _rebates.FirstOrDefault(x => x.Identifier == rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
