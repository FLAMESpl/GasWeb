namespace GasWeb.Domain
{
    internal static class PriceCalculator
    {
        internal static readonly decimal VAT = 0.23M;

        public static decimal CalculateFromRateForCubicMeter(decimal pricePerCubicMeter) => (pricePerCubicMeter * (1 + VAT)) / 1000;
    }
}
