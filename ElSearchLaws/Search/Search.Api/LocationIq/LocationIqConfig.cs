namespace NiceToGift.Shops.Infrastructure.LocationIq;

public class LocationIqConfig
{
    public const string SectionName = "LocationIqConfig";
    public string BaseUrl { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}