using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.GasStations
{
    public class AddGasStationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Location Location { get; set; }

        public long? FranchiseId { get; set; }
    }
}
