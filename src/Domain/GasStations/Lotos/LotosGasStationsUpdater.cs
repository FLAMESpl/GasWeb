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
        private readonly UserContextAuditMetadataProvider auditMetadataProvider;

        public LotosGasStationsUpdater(
            LotosGasStationsFetcher gasStationsFetcher,
            SystemFranchiseCollection franchiseCollection,
            GasWebDbContext dbContext,
            UserContextAuditMetadataProvider auditMetadataProvider)
        {
            this.gasStationsFetcher = gasStationsFetcher;
            this.franchiseCollection = franchiseCollection;
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task UpdateGasStations()
        {
            var gasStationNames = await gasStationsFetcher.GetLotosGasStations();
            var gasStationsQueryValues = string.Join(",", gasStationNames.Select((name, n) => $"({n}, '{name}')"));

            var missingGasStations = await dbContext.Database.GetDbConnection().QueryAsync<int>($@"
                SELECT n FROM (VALUES { gasStationsQueryValues }) as request (n, name)
                LEFT JOIN ""{ nameof(GasStation) }s"" stations
                ON stations.""{ nameof(GasStation.Name) }"" = request.name 
                    AND stations.""{ nameof(GasStation.FranchiseId) }"" = { franchiseCollection.Lotos }
                WHERE stations.""{ nameof(GasStation.Id) }"" IS NULL");

            foreach (var index in missingGasStations)
            {
                var gasStation = new GasStation(
                    name: gasStationNames[index],
                    latitude: 0,
                    longitude: 0,
                    franchiseId: franchiseCollection.Lotos,
                    maintainedBySystem: true);

                auditMetadataProvider.AddAuditMetadataToNewEntity(gasStation);
                dbContext.Add(gasStation);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
