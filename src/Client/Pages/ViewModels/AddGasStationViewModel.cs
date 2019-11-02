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

        public string Franchise { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public AddGasStationModel ToModel(IReadOnlyDictionary<long, string> dict) =>
            new AddGasStationModel
            {
                FranchiseId = string.IsNullOrEmpty(Franchise) ? default(long?) : dict.Single(x => x.Value == Franchise).Key,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                Name = Name
            };
    }
}
