using Dapper;
using GasWeb.Domain.Franchises;
using GasWeb.Domain.GasStations.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.GasStations.Lotos
{
    public interface ILotosGasStationsUpdater
    {
        Task UpdateGasStations();
    }

    internal class LotosGasStationsUpdater : ILotosGasStationsUpdater
    {
        private readonly LotosGasStationsFetcher gasStationsFetcher;
        private readonly SystemFranchiseCollection franchiseCollection;
        private readonly GasWebDbContext dbContext;
        private readonly IAuditMetadataProvider auditMetadataProvider;

        public LotosGasStationsUpdater(
            LotosGasStationsFetcher gasStationsFetcher,
            SystemFranchiseCollection franchiseCollection,
            GasWebDbContext dbContext,
            IAuditMetadataProvider auditMetadataProvider)
        {
            this.gasStationsFetcher = gasStationsFetcher;
            this.franchiseCollection = franchiseCollection;
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task UpdateGasStations()
        {
            var gasStations = await gasStationsFetcher.GetLotosGasStations();
            var gasStationsQueryValues = string.Join(",", gasStations.Select((x, n) => $"({n}, '{x.Name}')"));

            var missingGasStations = await dbContext.Database.GetDbConnection().QueryAsync<int>($@"
                SELECT n FROM (VALUES { gasStationsQueryValues }) as request (n, name)
                LEFT JOIN ""{ nameof(GasStation) }s"" stations
                ON stations.""{ nameof(GasStation.Name) }"" = request.name 
                    AND stations.""{ nameof(GasStation.FranchiseId) }"" = { franchiseCollection.Lotos }
                WHERE stations.""{ nameof(GasStation.Id) }"" IS NULL");

            foreach (var index in missingGasStations)
            {
                var missingGasStation = gasStations[index];
                var gasStation = new GasStation(
                    name: missingGasStation.Name,
                    addressLine1: missingGasStation.AddressLine1,
                    addressLine2: missingGasStation.AddressLine2,
                    franchiseId: franchiseCollection.Lotos,
                    maintainedBySystem: true);

                auditMetadataProvider.AddAuditMetadataToNewEntity(gasStation);
                dbContext.Add(gasStation);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
