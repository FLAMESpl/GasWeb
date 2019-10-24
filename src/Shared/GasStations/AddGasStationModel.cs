using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.GasStations
{
    public class AddGasStationModel
    {
        [Required]
        public Location Location { get; set; }
    }
}
