namespace BadBroker.Api.Models;

public class BestRatesPlan
{
    public DateTime BuyDate { get; set; }
    public DateTime SellDate { get; set; }
    public string Tool { get; set; }
    public double Revenue { get; set; }

}
