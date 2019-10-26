using System;

namespace GasWeb.Domain.Initialization
{
    internal class SystemUserMetadataProvider : IAuditMetadataProvider
    {
        private readonly long systemUserId;

        public SystemUserMetadataProvider(long systemUserId)
        {
            this.systemUserId = systemUserId;
        }

        public void AddAuditMetadataToNewEntity(AuditEntity entity)
        {
            entity.UpdateAuditMetadata(systemUserId, systemUserId, DateTime.UtcNow);
        }

        public void UpdateAuditMetadataInExistingEntiy(AuditEntity entity)
        {
            entity.UpdateAuditMetadata(entity.CreatedByUserId, systemUserId, DateTime.UtcNow);
        }
    }
}
