using GasWeb.Shared;

namespace GasWeb.Client.Shared
{
    public static class Extensions
    {
        public static string ToDisplayName(this FuelType fuelType)
        {
            switch (fuelType)
            {
                case FuelType.Petrol: return "Benzyna";
                case FuelType.Diesel: return "Diesel";
                case FuelType.Gas: return "Gaz";
                default: return fuelType.ToString();
            }
        }
    }
}
