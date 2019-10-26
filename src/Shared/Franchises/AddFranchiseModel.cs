using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Franchises
{
    public class AddFranchiseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
