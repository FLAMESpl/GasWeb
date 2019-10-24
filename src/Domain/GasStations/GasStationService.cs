using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.GasStations
{
    public interface IGasStationService
    {
        Task<GasStation> Get(long id);
        Task<PageResponse<GasStation>> GetList(int pageNumber, int pageSize);
        Task<long> Create(AddGasStationModel model);
        Task Update(long id, UpdateGasStationModel model);
        Task Delete(long id);
    }

    internal class GasStationService : IGasStationService
    {
        private readonly GasWebDbContext dbContext;
        private readonly AuditMetadataProvider auditMetadataProvider;

        public GasStationService(GasWebDbContext dbContext, AuditMetadataProvider auditMetadataProvider)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task<long> Create(AddGasStationModel model)
        {
            var gasStation = new Entities.GasStation(
                latitude: model.Location.Latitude, 
                longitude: model.Location.Longitude);

            auditMetadataProvider.AddAuditMetadataToNewEntity(gasStation);
            dbContext.GasStations.Add(gasStation);
            await dbContext.SaveChangesAsync();
            return gasStation.Id;
        }

        public async Task Delete(long id)
        {
            var gasStation = await dbContext.GasStations.GetAsync(id);
            dbContext.GasStations.Add(gasStation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<GasStation> Get(long id)
        {
            var gasStation = await dbContext.GasStations.GetAsync(id);
            return gasStation.ToContract();
        }

        public Task<PageResponse<GasStation>> GetList(int pageNumber, int pageSize)
        {
            return dbContext.GasStations.PickPageAsync(pageNumber, pageSize, x => x.ToContract());
        }

        public Task Update(long id, UpdateGasStationModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
