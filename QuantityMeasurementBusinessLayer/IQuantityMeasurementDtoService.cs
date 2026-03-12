namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC15 application-level service interface using DTOs.
    /// Built on top of the UC14 domain service.
    /// </summary>
    public interface IQuantityMeasurementDtoService
    {
        bool CompareQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity);

        QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit);

        QuantityDto AddQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit);

        QuantityDto SubtractQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity, string resultUnit);

        double DivideQuantities(QuantityDto firstQuantity, QuantityDto secondQuantity);
    }
}