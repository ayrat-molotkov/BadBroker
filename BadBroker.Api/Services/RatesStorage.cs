using BadBroker.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BadBroker.Api.Services;

public class RatesStorage
{
    public RatesStorage(IMemoryCache memoryCache)
    {        
        _memoryCache = memoryCache;
    }


    public async Task<Dictionary<DateTime, Dictionary<string, double>>> Get(DateTime startDate, DateTime endDate)
    {
        var result = new Dictionary<DateTime, Dictionary<string, double>>();

        DateTime date = startDate;        

        while (date <= endDate)
        {            
            if (_memoryCache.TryGetValue(date, out var rates))
            {                
                result.Add(date, (Dictionary<string, double>)rates);
            }

            date = date.AddDays(1);
        }

        return result;
    }


    public void Set(TimeseriesResponse response)
    { 
        foreach (var rate in response.Rates)
            _memoryCache.Set(rate.Key, rate.Value, CacheLifeTime);
    }


    private static TimeSpan CacheLifeTime => Constants.CacheLifeTime * 3;
    
    private readonly IMemoryCache _memoryCache;
}
