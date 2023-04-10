using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class FixedCashAmountRebateCalculator : IRebateCalculator
{
    private readonly Rebate _rebate;
    private readonly Product _product;

    public FixedCashAmountRebateCalculator(Rebate rebate, Product product)
    {
        _rebate = rebate;
        _product = product;
    }

    public bool CanCalculate()
    {
        return _rebate != null
            && _rebate.Incentive == IncentiveType.FixedCashAmount
            && _product != null
            && _product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount)
            && _rebate.Amount != 0;
    }

    public decimal Calculate()
    {
        if (!CanCalculate())
        {
            throw new InvalidOperationException($"Cannot calculate rebate {IncentiveType.FixedCashAmount}.");
        }

        return _rebate.Amount;
    }
}

