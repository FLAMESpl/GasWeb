using Microsoft.AspNetCore.Http;
using System;

namespace GasWeb.Domain
{
    internal interface IAuditMetadataProvider
    {
        void AddAuditMetadataToNewEntity(AuditEntity entity);
        void UpdateAuditMetadataInExistingEntiy(AuditEntity entity);
    }

    internal class UserContextAuditMetadataProvider : IAuditMetadataProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserContext userContext;

        public UserContextAuditMetadataProvider(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public void AddAuditMetadataToNewEntity(AuditEntity entity)
        {
            entity.UpdateAuditMetadata(
                createdByUserId: userContext.Id.Value,
                modifiedByUserId: userContext.Id.Value,
                lastModified: DateTime.UtcNow);
        }

        public void UpdateAuditMetadataInExistingEntiy(AuditEntity entity)
        {
            entity.UpdateAuditMetadata(
                createdByUserId: entity.CreatedByUserId,
                modifiedByUserId: userContext.Id.Value,
                lastModified: DateTime.UtcNow);
        }
    }
}
