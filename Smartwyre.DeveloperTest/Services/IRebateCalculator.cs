namespace Smartwyre.DeveloperTest.Services;

public interface IRebateCalculator
{
    bool CanCalculate();
    decimal Calculate();
}

