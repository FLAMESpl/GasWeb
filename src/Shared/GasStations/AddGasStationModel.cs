using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.GasStations
{
    public class AddGasStationModel
    {
        [Required]
        public string Name { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public long? FranchiseId { get; set; }
    }
}
