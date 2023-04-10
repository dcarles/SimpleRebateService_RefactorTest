using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public interface IRebateDataStore
{
    Rebate GetRebate(string identifier);
    void StoreCalculationResult(Rebate account, decimal rebateAmount);
}
