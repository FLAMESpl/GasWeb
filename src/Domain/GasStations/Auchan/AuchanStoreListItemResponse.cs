using System;
using System.Collections.Generic;
using System.Text;

namespace GasWeb.Domain.GasStations.Auchan
{
    internal class AuchanStoreListItemResponse
    {
        public string post_title { get; set; }
        public string street_address { get; set; }
        public string zip_code { get; set; }
        public string city { get; set; }
        public StoreUrlName store_url_name { get; set; }
    }

    internal class StoreUrlName
    {
        public string pl { get; set; }
    }
}
