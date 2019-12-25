using System;

namespace GasWeb.Server.Tests.Integration
{
    public static class Routes
    {
        public static readonly Uri Authentication = Add("api/auth");
        public static readonly Uri Comments = Add("api/comments");
        public static readonly Uri Dashboards = Add("api/dashboards");
        public static readonly Uri Franchises = Add("api/franchises");
        public static readonly Uri GasStations = Add("api/gas-stations");
        public static readonly Uri PriceSubmissions = Add("api/price-submissions");
        public static readonly Uri Schedulers = Add("api/schedulers");

        static Uri Add(string route) => new Uri(route, UriKind.Relative);
    }
}
