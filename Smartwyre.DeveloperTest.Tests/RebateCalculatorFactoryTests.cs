using Xunit;
using System;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Services;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateCalculatorFactoryTests
{
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;
    private readonly RebateCalculatorFactory _rebateCalculatorFactory;

    public RebateCalculatorFactoryTests()
    {
        _rebate = new Rebate();
        _product = new Product();
        _request = new CalculateRebateRequest();
        _rebateCalculatorFactory = new RebateCalculatorFactory();
    }

    [Fact]
    public void Create_FixedCashAmountIncentive_ReturnsFixedCashAmountRebateCalculator()
    {
        // Arrange
        _rebate.Incentive = IncentiveType.FixedCashAmount;

        // Act
        var rebateCalculator = _rebateCalculatorFactory.Create(_rebate, _product, _request);

        // Assert
        Assert.IsType<FixedCashAmountRebateCalculator>(rebateCalculator);
    }

    [Fact]
    public void Create_FixedRateRebateIncentive_ReturnsFixedRateRebateCalculator()
    {
        // Arrange
        _rebate.Incentive = IncentiveType.FixedRateRebate;

        // Act
        var rebateCalculator = _rebateCalculatorFactory.Create(_rebate, _product, _request);

        // Assert
        Assert.IsType<FixedRateRebateCalculator>(rebateCalculator);
    }

    [Fact]
    public void Create_AmountPerUomIncentive_ReturnsAmountPerUomRebateCalculator()
    {
        // Arrange
        _rebate.Incentive = IncentiveType.AmountPerUom;

        // Act
        var rebateCalculator = _rebateCalculatorFactory.Create(_rebate, _product, _request);

        // Assert
        Assert.IsType<AmountPerUomRebateCalculator>(rebateCalculator);
    }

    [Fact]
    public void Create_UnsupportedIncentiveType_ThrowsArgumentException()
    {
        // Arrange
        _rebate.Incentive = (IncentiveType)99;

        // Act and Assert
        Assert.Throws<ArgumentException>(() => _rebateCalculatorFactory.Create(_rebate, _product, _request));
    }
}
