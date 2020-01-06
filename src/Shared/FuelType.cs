using System;

namespace GasWeb.Shared
{
    [Flags]
    public enum FuelType
    {
        None = 0,
        Pb95 = 1,
        Pb98 = 2,
        Diesel = 4,
        DieselPremium = 8,
        Gas = 16,
        All = Pb95 | Pb98 | Diesel | DieselPremium | Gas
    }
}
