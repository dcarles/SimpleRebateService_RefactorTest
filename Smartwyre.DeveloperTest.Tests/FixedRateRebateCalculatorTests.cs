﻿using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedRateRebateCalculatorTests
{
    [Fact]
    public void CanCalculate_ReturnsTrue_WhenAllConditionsMet()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

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
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenIncentiveTypeIsNotFixedRateRebate()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenProductIsNull()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate };
        Product product = null;
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenProductDoesNotSupportFixedRateRebate()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Fact]
    public void CanCalculate_ReturnsFalse_WhenPercentageIsZero()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10m };
        var request = new CalculateRebateRequest { Volume = 5m };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var canCalculate = calculator.CanCalculate();

        // Assert
        canCalculate.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 100, 10, false)]    // Rebate percentage is zero
    [InlineData(10, 0, 10, false)]     // Product price is zero
    [InlineData(10, 100, 0, false)]    // Request volume is zero
    [InlineData(10, 100, 10, true)]    // All values are valid
    public void CanCalculate_ReturnsCorrectValue(decimal rebatePercentage, decimal productPrice, decimal requestVolume, bool expectedResult)
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = rebatePercentage };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = productPrice };
        var request = new CalculateRebateRequest { Volume = requestVolume };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var result = calculator.CanCalculate();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0, 100, 10)]    // Rebate percentage is zero
    [InlineData(10, 0, 10)]     // Product price is zero
    [InlineData(10, 100, 0)]    // Request volume is zero
    public void Calculate_ThrowsException_WhenCannotCalculate(decimal rebatePercentage, decimal productPrice, decimal requestVolume)
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = rebatePercentage };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = productPrice };
        var request = new CalculateRebateRequest { Volume = requestVolume };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => calculator.Calculate());
    }

    [Fact]
    public void Calculate_ReturnsCorrectValue_WhenAllValuesAreValid()
    {
        // Arrange
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 100 };
        var request = new CalculateRebateRequest { Volume = 10 };
        var calculator = new FixedRateRebateCalculator(rebate, product, request);

        // Act
        var result = calculator.Calculate();

        // Assert
        result.Should().Be(rebate.Percentage * product.Price * request.Volume);
    }
}