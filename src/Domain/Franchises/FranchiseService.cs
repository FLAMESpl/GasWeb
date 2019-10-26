using GasWeb.Shared;
using GasWeb.Shared.Franchises;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises
{
    public interface IFranchiseService
    {
        Task<Franchise> Get(long id);
        Task<PageResponse<Franchise>> GetList(int pageNumber, int pageSize);
        Task<long> Create(AddFranchiseModel model);
        Task Update(long id, UpdateFranchiseModel model);
        Task Delete(long id);
    }

    internal class FranchiseService : IFranchiseService
    {
        private readonly GasWebDbContext dbContext;
        private readonly UserContextAuditMetadataProvider auditMetadataProvider;

        public FranchiseService(GasWebDbContext dbContext, UserContextAuditMetadataProvider auditMetadataProvider)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task<long> Create(AddFranchiseModel model)
        {
            var franchise = new Entities.Franchise(model.Name, false);
            auditMetadataProvider.AddAuditMetadataToNewEntity(franchise);
            dbContext.Franchises.Add(franchise);
            await dbContext.SaveChangesAsync();
            return franchise.Id;
        }

        public async Task Delete(long id)
        {
            var franchise = await dbContext.Franchises.GetAsync(id);
            dbContext.Remove(franchise);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Franchise> Get(long id)
        {
            var franchise = await dbContext.Franchises.Include(x => x.WholesalePrices).GetAsync(x => x.Id == id);
            return franchise.ToContract();
        }

        public Task<PageResponse<Franchise>> GetList(int pageNumber, int pageSize)
        {
            return dbContext.Franchises.Include(x => x.WholesalePrices).PickPageAsync(pageNumber, pageSize, x => x.ToContract());
        }

        public async Task Update(long id, UpdateFranchiseModel model)
        {
            var franchise = await dbContext.Franchises.GetAsync(id);
            auditMetadataProvider.UpdateAuditMetadataInExistingEntiy(franchise);
            franchise.Update(model);
            await dbContext.SaveChangesAsync();
        }
    }
}
