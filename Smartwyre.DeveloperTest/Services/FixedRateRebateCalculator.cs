using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class FixedRateRebateCalculator : IRebateCalculator
{
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;

    public FixedRateRebateCalculator(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        _rebate = rebate;
        _product = product;
        _request = request;
    }

    public bool CanCalculate()
    {
        return _rebate != null
            && _rebate.Incentive == IncentiveType.FixedRateRebate
            && _product != null
            && _product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
            && _rebate.Percentage != 0
            && _product.Price != 0
            && _request.Volume != 0;
    }

    public decimal Calculate()
    {
        if (!CanCalculate())
        {
            throw new InvalidOperationException($"Cannot calculate rebate {IncentiveType.FixedRateRebate}.");
        }

        return _product.Price * _rebate.Percentage * _request.Volume;
    }
}
