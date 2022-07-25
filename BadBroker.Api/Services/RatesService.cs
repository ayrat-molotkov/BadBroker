using BadBroker.Api.Models;
using Microsoft.Extensions.Logging;

namespace BadBroker.Api.Services;

public class RatesService : IRatesService
{
    public RatesService(ExchangeRatesClient client, RatesStorage ratesStorage)
    {
        _client = client;
        _ratesStorage = ratesStorage;
    }


    public async Task<BestRatesPlan> GetBestRatesPlan(DateTime startDate, DateTime endDate, int moneyUsd, CancellationToken cancellationToken)
    {        
        if ((endDate - startDate).TotalDays > 60)
            throw new Exception("The specified historical period cannot exceed 2 months (60 days).");

        var rates = await GetRatesAsync(startDate, endDate, cancellationToken);

        var currencyProfits = new List<(double currencyRevenue, DateTime sellDate, DateTime buyDate, string currency)>();

        foreach(var currency in Constants.Symbols)
        {
            var currencyRates = GetCurrencyRates(currency);
            var sellRate = currencyRates.OrderBy(x => x.price).First();
            var buyRate = currencyRates.OrderByDescending(x => x.price).First();            

            if (sellRate.date > buyRate.date)
            {
                var currencyRevenue = GetProfitAfterBuyAndSell(buyRate.price, sellRate.price) - GetBrokerComission(buyRate.date, sellRate.date);

                currencyProfits.Add((currencyRevenue, sellRate.date, buyRate.date, currency));
            }                
        }

        var bestProfit = currencyProfits.OrderByDescending(x => x.currencyRevenue).First();

        return new BestRatesPlan()
        {
            BuyDate = bestProfit.buyDate,
            SellDate = bestProfit.sellDate,
            Revenue = bestProfit.currencyRevenue - moneyUsd,
            Tool = bestProfit.currency
        };


        List<(DateTime date, double price)> GetCurrencyRates(string currency)
            => rates.Select(x => (x.Key, x.Value.First(y => y.Key == currency).Value)).ToList();


        double GetProfitAfterBuyAndSell(double buyPrice, double sellPrice)
            => buyPrice * moneyUsd / sellPrice;


        double GetBrokerComission(DateTime buyDate, DateTime sellDate)
            => (sellDate - buyDate).TotalDays * Constants.BrokerFee;        
    }


    private async Task<Dictionary<DateTime, Dictionary<string, double>>> GetRatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var cacheRates = await _ratesStorage.Get(startDate, endDate);        

        if (cacheRates.Count != (endDate - startDate).Days + 1)
        {
            var notCachedDates = Range(startDate, endDate).Where(x => !cacheRates.Select(y => y.Key).Contains(x));
            var timeseries = await _client.GetTimeseries(notCachedDates.Min(), notCachedDates.Max(), cancellationToken);

            if (timeseries is not null)
            {
                _ratesStorage.Set(timeseries);

                foreach (var rate in timeseries.Rates)
                    cacheRates.Add(rate.Key, rate.Value);
            }
            else
            {
                _logger.LogError("Timeseries not found");
            }            
        }        

        return cacheRates;


        IEnumerable<DateTime> Range(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }
    }


    private readonly ExchangeRatesClient _client;
    private readonly RatesStorage _ratesStorage;
    private readonly ILogger<RatesService> _logger;
}
