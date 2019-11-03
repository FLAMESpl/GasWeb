using GasWeb.Shared.Comments;

namespace GasWeb.Domain.Comments
{
    internal static class TypeMaps
    {
        public static Comment ToContract(this Entities.Comment domain) =>
            new Comment
            {
                Content = domain.Content,
                CreatedAt = domain.CreatedAt,
                CreatedByUserId = domain.CreatedByUserId,
                Id = domain.Id,
                LastModifiedAt = domain.LastModified,
                LastModifiedByUserId = domain.ModifiedByUserId,
                Tag = domain.Tag,
                WasEdited = domain.WasEdited,
                SubjectId = domain.SubjectId
            };
    }
}
