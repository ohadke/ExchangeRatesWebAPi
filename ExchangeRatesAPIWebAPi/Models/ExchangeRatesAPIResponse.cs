using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace TestSpinomenal.Models
{
    public class ExchangeRatesAPIResponse
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string,double> Rates { get; set; }
    }

}
