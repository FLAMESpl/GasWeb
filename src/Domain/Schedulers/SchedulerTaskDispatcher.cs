using GasWeb.Domain.Franchises.Bp;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.Franchises.Orlen;
using GasWeb.Domain.GasStations.Auchan;
using GasWeb.Domain.GasStations.Lotos;
using GasWeb.Domain.PriceSubmissions.Auchan;
using System;
using System.Threading.Tasks;

namespace GasWeb.Domain.Schedulers
{
    internal class SchedulerTaskDispatcher
    {
        private readonly ILotosWholesalePriceUpdater lotosWholesalePriceUpdater;
        private readonly IOrlenWholesalePriceUpdater orlenWholesalePriceUpdater;
        private readonly IBpWholesalePriceUpdater bpWholesalePriceUpdater;
        private readonly ILotosGasStationsUpdater lotosGasStationsUpdater;
        private readonly IAuchanGasStationsUpdater auchanGasStationsUpdater;
        private readonly IAuchanGasStationsPricesUpdater auchanGasStationsPricesUpdater;

        public SchedulerTaskDispatcher(
            ILotosWholesalePriceUpdater lotosWholesalePriceUpdater,
            IOrlenWholesalePriceUpdater orlenWholesalePriceUpdater,
            IBpWholesalePriceUpdater bpWholesalePriceUpdater,
            ILotosGasStationsUpdater lotosGasStationsUpdater,
            IAuchanGasStationsUpdater auchanGasStationsUpdater,
            IAuchanGasStationsPricesUpdater auchanGasStationsPricesUpdater)
        {
            this.lotosWholesalePriceUpdater = lotosWholesalePriceUpdater;
            this.orlenWholesalePriceUpdater = orlenWholesalePriceUpdater;
            this.bpWholesalePriceUpdater = bpWholesalePriceUpdater;
            this.lotosGasStationsUpdater = lotosGasStationsUpdater;
            this.auchanGasStationsUpdater = auchanGasStationsUpdater;
            this.auchanGasStationsPricesUpdater = auchanGasStationsPricesUpdater;
        }

        public Task ExecuteTask(long id)
        {
            return id switch
            {
                SchedulersCollection.RefreshPricesLotos => lotosWholesalePriceUpdater.UpdateWholesalePrices(),
                SchedulersCollection.RefreshPricesOrlen => orlenWholesalePriceUpdater.UpdateWholesalePrices(),
                SchedulersCollection.RefreshPricesBp => bpWholesalePriceUpdater.UpdateWholesalePrices(),
                SchedulersCollection.RefreshGasStationsLotos => lotosGasStationsUpdater.UpdateGasStations(),
                SchedulersCollection.RefreshGasStationsAuchan => auchanGasStationsUpdater.UpdateGasStations(),
                SchedulersCollection.RefreshPricesAuchan => auchanGasStationsPricesUpdater.UpdatePrices(),
                _ => throw new ArgumentException($"Unknown task: {id}", nameof(id))
            };
        }
    }
}
