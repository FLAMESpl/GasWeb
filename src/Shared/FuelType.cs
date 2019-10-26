using System;

namespace GasWeb.Shared
{
    [Flags]
    public enum FuelType
    {
        None = 0,
        Petrol = 1,
        Diesel = 2,
        Gas = 4,
        All = Petrol | Diesel | Gas
    }
}
