using System;
using System.Collections.Generic;
using System.Text;

namespace GasWeb.Shared.Comments
{
    public class Comment
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long CreatedByUserId { get; set; }
        public long LastModifiedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public CommentTag Tag { get; set; }
        public bool WasEdited { get; set; }
        public string SubjectId { get; set; }
    }
}
