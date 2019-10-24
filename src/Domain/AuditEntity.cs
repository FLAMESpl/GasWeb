using System;

namespace GasWeb.Domain
{
    internal interface IEntity
    {
        long Id { get; }
    }

    internal abstract class AuditEntity : IEntity
    {
        protected AuditEntity()
        {
        }

        protected AuditEntity(long id, long createdByUserId, long modifiedByUserId, DateTime lastModified)
        {
            Id = id;
            CreatedByUserId = createdByUserId;
            ModifiedByUserId = modifiedByUserId;
            LastModified = lastModified;
        }

        public long Id { get; protected set; }
        public long CreatedByUserId { get; protected set; }
        public long ModifiedByUserId { get; protected set; }
        public DateTime LastModified { get; protected set; }

        internal void UpdateAuditMetadata(long createdByUserId, long modifiedByUserId, DateTime lastModified)
        {
            CreatedByUserId = createdByUserId;
            ModifiedByUserId = modifiedByUserId;
            LastModified = lastModified;
        }
    }
}
