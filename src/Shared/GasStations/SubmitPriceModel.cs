﻿using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.GasStations
{
    public class SubmitPriceModel
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public long GasStationId { get; set; }
    }
}
