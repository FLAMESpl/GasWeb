using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Comments
{
    public class AddCommentModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public CommentTag Tag { get; set; }

        [Required]
        public string SubjectId { get; set; }
    }
}
