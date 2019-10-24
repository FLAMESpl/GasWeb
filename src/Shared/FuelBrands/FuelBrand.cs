namespace GasWeb.Shared.FuelBrands
{
    public class FuelBrand
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public FuelType Type { get; set; }
        public DocumentStatus Status { get; set; }
    }
}
