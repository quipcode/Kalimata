using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace currencyRateUpdate.APIModels
{
    class RateRetrieval
    {
        public static HttpClient service = null;
        public async Task Connection()
        {
            using (var service = new HttpClient())
            {
                // prepare address calls
                string apiBaseAddress = "http://data.fixer.io/api/";
                string apiConnectionString = "http://data.fixer.io/api/latest?access_key=3edd3e01c73d7f004c08acabe3100d4f&symbols=USD,CAD";


                // prep HttpClient call for JSON
                service.BaseAddress = new Uri(apiBaseAddress);
                service.DefaultRequestHeaders.Accept.Clear();
                service.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public static async Task GetRatesAsync(string apiConnectionString)
        {
            // Retrieve JSON as a string
            string message = await service.GetStringAsync(apiConnectionString);

            // Convert to JSON format
            dynamic information = JsonConvert.DeserializeObject(message);
        }
    }
}
