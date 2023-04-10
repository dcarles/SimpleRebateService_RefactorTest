using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    public IRebateCalculator Create(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                return new FixedCashAmountRebateCalculator(rebate, product);
            case IncentiveType.FixedRateRebate:
                return new FixedRateRebateCalculator(rebate, product, request);
            case IncentiveType.AmountPerUom:
                return new AmountPerUomRebateCalculator(rebate, product, request);
            default:
                throw new ArgumentException($"Unsupported incentive type: {rebate.Incentive}");
        }
    }
}
