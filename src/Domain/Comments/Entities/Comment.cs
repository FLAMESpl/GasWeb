using GasWeb.Shared.Comments;
using System;

namespace GasWeb.Domain.Comments.Entities
{
    internal class Comment : AuditEntity
    {
        public Comment(string content, DateTime createdAt, CommentTag tag, string subjectId)
        {
            Content = content;
            CreatedAt = createdAt;
            Tag = tag;
            SubjectId = subjectId;
            WasEdited = false;
        }

        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public CommentTag Tag { get; private set; }
        public string SubjectId { get; private set; }
        public bool WasEdited { get; private set; }

        internal void Update(UpdateCommentModel model)
        {
            Content = model.Content;
            WasEdited = true;
        }
    }
}
