using BadBroker.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BadBroker.Api;

public class ExchangeRatesClient
{
    public ExchangeRatesClient(IHttpClientFactory httpClientFactory, ILogger<ExchangeRatesClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }


    public Task<TimeseriesResponse> GetTimeseries(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        const string endpointUrl = "exchangerates_data/timeseries";
        var requestUrl = $"{endpointUrl}?start_date={startDate.ToString("yyyy-MM-dd")}&end_date={endDate.ToString("yyyy-MM-dd")}&symbols={string.Join(",", Constants.Symbols)}&base={Constants.Base}";

        try
        {
            return Get<TimeseriesResponse>(new Uri(requestUrl, UriKind.Relative), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed get timeseries", ex);
            return default;
        }
    }


    private Task<TResponse> Get<TResponse>(Uri url, CancellationToken cancellationToken)
            => Send<TResponse>(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken);


    private async Task<TResponse> Send<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(Constants.HttpClientName);
        using var response = await client.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        const string errorMessageTemplate = "SupplierStatusCode: {0}, Errors: `{1}`";

        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        return default;
    }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExchangeRatesClient> _logger;
}
