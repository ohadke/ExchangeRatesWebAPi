using Newtonsoft.Json;
using System;

namespace TestSpinomenal.Models
{
    public class CurrencyRequest
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("targetcurrency")]
        public string TargetCurrency { get; set; }

        [JsonProperty("exchangedate")]
        public DateTime ExchangeDate { get; set; }

        [JsonProperty("toconvert")]
        public ToConvert[] ToConvert { get; set; }
    }

    public class ToConvert
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
