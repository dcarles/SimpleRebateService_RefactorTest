using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
    private readonly Mock<IProductDataStore> _productDataStoreMock;
    private readonly Mock<IRebateCalculatorFactory> _rebateCalculatorFactoryMock;
    private readonly Mock<IRebateCalculator> _mockRebateCalculator; 

    public RebateServiceTests()
    {
        _rebateDataStoreMock = new Mock<IRebateDataStore>();
        _productDataStoreMock = new Mock<IProductDataStore>();
        _rebateCalculatorFactoryMock = new Mock<IRebateCalculatorFactory>();
        _mockRebateCalculator = new Mock<IRebateCalculator>();
    }

    [Fact]
    public void Calculate_ReturnsSuccessTrue_WhenRebateAndProductAreNotNull()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "1",
            ProductIdentifier = "1",
            Volume = 10
        };
        var rebate = new Rebate { Identifier = "1", Incentive = IncentiveType.FixedCashAmount, Amount = 10 };
        var product = new Product { Identifier = "1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 2.5m };
        _rebateDataStoreMock.Setup(ds => ds.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(ds => ds.GetProduct(request.ProductIdentifier)).Returns(product);
        _mockRebateCalculator.Setup(c => c.CanCalculate()).Returns(true);
        _rebateCalculatorFactoryMock.Setup(cf => cf.Create(rebate, product, request)).Returns(_mockRebateCalculator.Object);
        var service = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object, _rebateCalculatorFactoryMock.Object);

        // Act
        var result = service.Calculate(request);

        // Assert
        result.Success.Should().BeTrue();      
        _mockRebateCalculator.Verify(c => c.CanCalculate(), Times.Once);
        _mockRebateCalculator.Verify(c => c.Calculate(), Times.Once);
        _rebateDataStoreMock.Verify(ds => ds.StoreCalculationResult(rebate, It.IsAny<decimal>()), Times.Once);
    }

    [Fact]
    public void Calculate_ReturnsSuccessFalse_WhenRebateIsNull()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "1",
            ProductIdentifier = "1",
            Volume = 10
        };
        Rebate rebate = null;
        var product = new Product { Identifier = "1", SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Price = 2.5m };
        _rebateDataStoreMock.Setup(ds => ds.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(ds => ds.GetProduct(request.ProductIdentifier)).Returns(product);
        var service = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object, _rebateCalculatorFactoryMock.Object);

        // Act
        var result = service.Calculate(request);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void Calculate_ReturnsSuccessFalse_WhenProductIsNull()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "1",
            ProductIdentifier = "1",
            Volume = 10
        };
        var rebate = new Rebate { Identifier = "1", Incentive = IncentiveType.FixedCashAmount, Amount = 1m };
        Product product = null;    
        _rebateDataStoreMock.Setup(ds => ds.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(ds => ds.GetProduct(request.ProductIdentifier)).Returns(product);
        var service = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object, _rebateCalculatorFactoryMock.Object);

        // Act
        var result = service.Calculate(request);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void Calculate_WithInvalidRebateCalculator_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "valid",
            ProductIdentifier = "valid",
            Volume = 10
        };
        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Percentage = 5 };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        _rebateDataStoreMock.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productDataStoreMock.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);
        _rebateCalculatorFactoryMock.Setup(x => x.Create(rebate, product, request)).Throws<ArgumentException>();
        var service = new RebateService(_rebateDataStoreMock.Object, _productDataStoreMock.Object, _rebateCalculatorFactoryMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.Calculate(request));
        _rebateCalculatorFactoryMock.Verify(x => x.Create(rebate, product, request), Times.Once);
        _mockRebateCalculator.Verify(c => c.CanCalculate(), Times.Never);
        _mockRebateCalculator.Verify(c => c.Calculate(), Times.Never);
    }

}
