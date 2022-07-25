using BadBroker.Api.Models;

namespace BadBroker.Api.Services;

public interface IRatesService
{
    Task<BestRatesPlan> GetBestRatesPlan(DateTime startDate, DateTime endDate, int moneyUsd, CancellationToken cancellationToken);
}
