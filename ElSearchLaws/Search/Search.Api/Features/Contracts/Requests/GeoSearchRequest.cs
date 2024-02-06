namespace Search.Api.Features.Contracts.Requests;

public class GeoSearchRequest
{
    public double Lat { get; set; }
    public double Long { get; set; }
    public long Distance { get; set; }
}