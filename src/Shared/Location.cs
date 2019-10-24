using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared
{
    public class Location
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
