using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TestSpinomenal.Models;

namespace TestSpinomenal.Controllers
{

    [Route("api")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        // mapping between the exchangeprovidername to the URL
        private Dictionary<string, string> ExchangeProviders = new Dictionary<string, string>()
        {
            {"exchangeratesapi", "http://data.fixer.io/api/latest?access_key=dab0994e9a5e5d0defc05c09c0e49094&format=1"},
            {"exchangerateshost","https://api.exchangerate.host/latest" }
        };

        [HttpGet("getproviders")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return ExchangeProviders.Keys;
        }


        [HttpPost("{exchangerate}")]
        public ActionResult<double> Post(CurrencyRequest currencyRequest)
        {
            string uri;
            if (ExchangeProviders.TryGetValue(currencyRequest.Provider, out uri))
            {
                try
                {
                   var convertedSum= getConvertedSum(uri, currencyRequest.ToConvert);
                    return Ok(convertedSum);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Unkown Provider");
            }
        }

        // Return the sum amount of all Amounts in “ToConvert” list converted to the TargetCurrency
        private double getConvertedSum(string uri, ToConvert[] toConvertList)
        {
            double totalSum = 0;
            
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode) {
                var responseString = response.Content.ReadAsStringAsync().Result;
                var exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesAPIResponse>(responseString);
                var rates = exchangeRates.Rates;
                foreach (var convert in toConvertList) {
                    if (rates.TryGetValue(convert.Currency, out double rate))
                    {
                        totalSum += convert.Amount * rate;
                    }
                    else
                    {
                        throw new Exception(string.Format("Request with unknown currency: {0}", convert.Currency));
                    }
                }
                client.Dispose();
                return totalSum;
            } 
            else
            {
                throw new Exception("Failed to reach provider");
            }
        }
    }

}
