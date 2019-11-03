using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Comments
{
    public class UpdateCommentModel
    {
        [Required]
        public string Content { get; set; }
    }
}
