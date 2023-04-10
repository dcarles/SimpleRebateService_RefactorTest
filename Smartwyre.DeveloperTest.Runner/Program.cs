using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

//Dependency Injection
var serviceProvider = new ServiceCollection()
            .AddSingleton<IProductDataStore, ProductDataStore>()
            .AddSingleton<IRebateDataStore, RebateDataStore>()
            .AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>()
            .AddSingleton<IRebateCalculator, FixedRateRebateCalculator>()
            .AddSingleton<IRebateCalculator, FixedCashAmountRebateCalculator>()
            .AddSingleton<IRebateCalculator, AmountPerUomRebateCalculator>()
            .AddSingleton<IRebateService, RebateService>()
            .BuildServiceProvider();

// Prompt the user for the required input values
Console.Write("Enter the product identifier (Mangoes, Bananas, Rice Or Sugarcane): ");
var productIdentifier = Console.ReadLine();

Console.Write("Enter the rebate identifier (AmountPerUom, FixedCashAmount, FixedRateRebate): ");
var rebateIdentifier = Console.ReadLine();

Console.Write("Enter the volume: ");
var volumeString = Console.ReadLine();
decimal volume;

while (!decimal.TryParse(volumeString, out volume))
{
    Console.WriteLine("Invalid volume, please enter a valid number.");
    volumeString = Console.ReadLine();
}

// Create the request
var request = new CalculateRebateRequest
{
    ProductIdentifier = productIdentifier,
    RebateIdentifier = rebateIdentifier,
    Volume = volume
};

//Get RebateService and Calculate Rebate
var rebateService = serviceProvider.GetService<IRebateService>();
var result = rebateService.Calculate(request);

// Print the result
if (result.Success)
{
    Console.WriteLine($"Rebate calculated successfully: {result.Amount:C}");
}
else
{
    Console.WriteLine("Unable to calculate rebate.");
}

Console.WriteLine("Press any key to terminate");
Console.ReadLine();
