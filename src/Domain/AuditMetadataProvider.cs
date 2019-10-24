using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace GasWeb.Domain
{
    internal class AuditMetadataProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuditMetadataProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void AddAuditMetadataToNewEntity(AuditEntity entity)
        {
            var userId = GetCurrentUserId();
            entity.UpdateAuditMetadata(
                createdByUserId: userId,
                modifiedByUserId: userId,
                lastModified: DateTime.UtcNow);
        }

        public void UpdateAuditMetadataInExistingEntiy(AuditEntity entity)
        {
            var userId = GetCurrentUserId();
            entity.UpdateAuditMetadata(
                createdByUserId: entity.CreatedByUserId,
                modifiedByUserId: userId,
                lastModified: DateTime.UtcNow);
        }

        private long GetCurrentUserId()
        {
            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(claim.Value);
        }
    }
}
