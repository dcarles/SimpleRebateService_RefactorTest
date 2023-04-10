using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedCashAmountRebateCalculatorTests
{
    [Fact]
    public void CanCalculate_ReturnsTrue_WhenAllConditionsMet()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeTrue();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenRebateIsNull()
    {
        // Arrange
        Rebate rebate = null;
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 10m };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenIncentiveTypeIsNotFixedCashAmount()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 10m };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenProductIsNull()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount };
        Product product = null;
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenProductDoesNotSupportFixedCashAmount()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenAmountIsZero()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 0 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 10m };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();      
    }

    [Theory]
    [InlineData(0, 100, false)]    // Amount is zero
    [InlineData(10, 100, true)]    // All values are valid
    public void CanCalculate_ReturnsCorrectValue(decimal amount, decimal productPrice, bool expectedResult)
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = amount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = productPrice };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var result = calculator.CanCalculate();

        // Assert
        result.Should().Be(expectedResult);    
    }

  [Fact]
    public void Calculate_ThrowsException_WhenCannotCalculate()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 0 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };    
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => calculator.Calculate());
    }

    [Fact]
    public void Calculate_ReturnsCorrectValue_WhenAllValuesAreValid()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 10 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 100 };
        var calculator = new FixedCashAmountRebateCalculator(rebate, product);

        // Act
        var result = calculator.Calculate();

        // Assert
        result.Should().Be(rebate.Amount);
    }
}