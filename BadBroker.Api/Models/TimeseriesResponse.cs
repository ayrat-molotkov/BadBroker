using Newtonsoft.Json;

namespace BadBroker.Api.Models;

public class TimeseriesResponse
{
    public bool Success { get; set; }
    public bool Timeseries { get; set; }

    [JsonProperty("start_date")]
    public DateTime StartDate { get; set; }

    [JsonProperty("end_date")]
    public DateTime EndDate { get; set; }
    public Dictionary<DateTime, Dictionary<string, double>> Rates { get; set; }
}
