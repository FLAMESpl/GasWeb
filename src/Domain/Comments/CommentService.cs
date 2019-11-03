using GasWeb.Shared;
using GasWeb.Shared.Comments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Comments
{
    public interface ICommentService
    {
        Task<long> Create(AddCommentModel model);
        Task Update(long id, UpdateCommentModel model);
        Task Delete(long id);
        Task<Comment> Get(long id);
        Task<PageResponse<Comment>> GetList(CommentTag commentTag, string subjectId, int pageNumber, int pageSize);
    }

    internal class CommentService : ICommentService
    {
        private readonly GasWebDbContext dbContext;
        private readonly IAuditMetadataProvider auditMetadataProvider;

        public CommentService(GasWebDbContext dbContext, IAuditMetadataProvider auditMetadataProvider)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task<long> Create(AddCommentModel model)
        {
            var comment = new Entities.Comment(
                content: model.Content,
                createdAt: DateTime.UtcNow,
                tag: model.Tag,
                subjectId: model.SubjectId);

            auditMetadataProvider.AddAuditMetadataToNewEntity(comment);
            dbContext.Add(comment);
            await dbContext.SaveChangesAsync();
            return comment.Id;
        }

        public async Task Delete(long id)
        {
            var comment = await dbContext.Comments.GetAsync(id);
            dbContext.Remove(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Comment> Get(long id)
        {
            var comment = await dbContext.Comments.GetAsync(id);
            return comment.ToContract();
        }

        public Task<PageResponse<Comment>> GetList(CommentTag commentTag, string subjectId, int pageNumber, int pageSize)
        {
            return dbContext.Comments
                .Where(x => x.Tag == commentTag && x.SubjectId == subjectId)
                .OrderByDescending(x => x.CreatedAt)
                .PickPageAsync(pageNumber, pageSize, x => x.ToContract());
        }

        public async Task Update(long id, UpdateCommentModel model)
        {
            var comment = await dbContext.Comments.GetAsync(id);
            comment.Update(model);
            await dbContext.SaveChangesAsync();
        }
    }
}
