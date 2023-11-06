using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BTCPayServer.Rating;
using Newtonsoft.Json.Linq;

namespace BTCPayServer.Services.Rates
{
    public class CustomRateProvider : IRateProvider
    {
        private readonly HttpClient _httpClient;
        public CustomRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public RateSourceInfo RateSourceInfo => new RateSourceInfo("custom", "Custom", "https://customrates.local/api/price?from_currency=BTC&to_currency=Bits");

        public async Task<PairRate[]> GetRatesAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://customrates.local/api/price?from_currency=BTC&to_currency=Bits", cancellationToken);
            var jobj = await response.Content.ReadAsAsync<JObject>(cancellationToken);
            var value = jobj["public_price"]["to_price"].Value<decimal>();
            return new[] { new PairRate(new CurrencyPair("BTC", "BITS"), new BidAsk(value)) };
        }
    }
}
