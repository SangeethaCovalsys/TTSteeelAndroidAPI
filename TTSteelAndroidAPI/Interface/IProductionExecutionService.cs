using static TTSteelAndroidAPI.Model.ProductionExecution.Production;

namespace TTSteelAndroidAPI.Interface
{
    public interface IProductionExecutionService
    {
       Task<UnitCountsDto> GetUnitCountsAsync();
    }
}
