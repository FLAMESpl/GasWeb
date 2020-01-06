using GasWeb.Shared;

namespace GasWeb.Client.Shared
{
    public static class Extensions
    {
        public static string ToDisplayName(this FuelType fuelType)
        {
            switch (fuelType)
            {
                case FuelType.Pb95: return "Pb95";
                case FuelType.Pb98: return "Pb98";
                case FuelType.Diesel: return "ON";
                case FuelType.DieselPremium: return "ON+";
                case FuelType.Gas: return "LPG";
                default: return fuelType.ToString();
            }
        }

        public static string ToDisplayAmount(this decimal price)
        {
            return price.ToString("0.00");
        }
    }
}
