namespace BadBroker.Api;

public static class Constants
{
    public static readonly string[] Symbols = new string[] { "RUB", "EUR", "GBP", "JPY", "USD" };
    public const string Base = "USD";
    public const string HttpClientName = "ExchangeRatesClient";
    public const double BrokerFee = 1;
    public static readonly TimeSpan CacheLifeTime = TimeSpan.FromMinutes(15);
    public const string CacheName = "RatesCache";
}
