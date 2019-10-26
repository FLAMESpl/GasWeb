using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GasWeb.Client.Pages.ViewModels
{
    public class AddGasStationViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Location Location { get; set; }

        public string Franchise { get; set; }

        public AddGasStationModel ToModel(IReadOnlyDictionary<long, string> dict) =>
            new AddGasStationModel
            {
                FranchiseId = string.IsNullOrEmpty(Franchise) ? default(long?) : dict.Single(x => x.Value == Franchise).Key,
                Location = Location,
                Name = Name
            };
    }
}
