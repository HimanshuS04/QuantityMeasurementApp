namespace QuantityMeasurementApp
{
    /// <summary>
    /// Single DTO-based service interface for quantity measurement.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        bool CompareQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity);

        QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit);

        QuantityDto AddQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit);

        QuantityDto SubtractQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit);

        double DivideQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity);
    }
}