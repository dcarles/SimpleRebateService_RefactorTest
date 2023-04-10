using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class AmountPerUomRebateCalculator : IRebateCalculator
{
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;

    public AmountPerUomRebateCalculator(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        _rebate = rebate;
        _product = product;
        _request = request;
    }

    public bool CanCalculate()
    {
        return _rebate != null
            && _rebate.Incentive == IncentiveType.AmountPerUom
            && _product != null
            && _product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom)
            && _rebate.Amount != 0
            && _request.Volume != 0;
    }

    public decimal Calculate()
    {
        if (!CanCalculate())
        {
            throw new InvalidOperationException($"Cannot calculate rebate {IncentiveType.AmountPerUom}.");
        }

        return _rebate.Amount * _request.Volume;
    }
}

