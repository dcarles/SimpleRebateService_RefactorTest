using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateCalculatorFactory _rebateCalculatorFactory;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore,
                         IRebateCalculatorFactory rebateCalculatorFactory)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _rebateCalculatorFactory = rebateCalculatorFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);
        var result = new CalculateRebateResult { Success = true };

        if (rebate == null || product == null)
        {
            result.Success = false;
            return result;
        }

        var rebateCalculator = _rebateCalculatorFactory.Create(rebate, product, request);

        if (!rebateCalculator.CanCalculate())
        {
            result.Success = false;
            return result;
        }

        var rebateAmount = rebateCalculator.Calculate();
        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return result;
    }
}

