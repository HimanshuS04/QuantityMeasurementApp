using QuantityService.Models;

namespace QuantityService.BusinessLogic
{
    public interface IQuantityMeasurementService
    {
        bool CompareQuantities(QuantityDto first, QuantityDto second);
        QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit);
        QuantityDto AddQuantities(QuantityDto first, QuantityDto second, string resultUnit);
        QuantityDto SubtractQuantities(QuantityDto first, QuantityDto second, string resultUnit);
        double DivideQuantities(QuantityDto first, QuantityDto second);
    }
}