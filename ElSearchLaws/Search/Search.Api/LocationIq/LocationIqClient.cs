using FluentResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NiceToGift.BuildingBlocks.Infrastructure.HttpHandler;
using NiceToGift.Shops.Infrastructure.LocationIq;

namespace Search.Api.LocationIq;

public class LocationIqClient : ILocationClient
{
    
    private readonly LocationIqConfig _locationIqConfig;
    private readonly IHttpClientHandler _httpClientHandler;

    public LocationIqClient(IOptions<LocationIqConfig> options, IHttpClientHandler httpClientHandler)
    {
        _httpClientHandler = httpClientHandler;
        _locationIqConfig = options.Value;
    }

    public async Task<Result<Location>> GetAddressLocation(Address address)
    {
        var httpResult = await _httpClientHandler.SendAsync(AssembleLocationRequest(address));
        var responseContent = await httpResult.Value.Content.ReadAsStringAsync(new CancellationToken());
        var locationJson = JsonConvert.DeserializeObject<List<LocationJsonResponse>>(responseContent);
        if (locationJson is null || locationJson.Count == 0)
            return Result.Fail(BaseErrors.NotFound("Location not found."));
        if (double.TryParse(locationJson[0].Lon, out var lon) && double.TryParse(locationJson[0].Lat, out var lat))
            return new Location(lon, lat);
        return Result.Fail(BaseErrors.NotFound("Location not found."));
    }
    
    private  HttpRequestMessage AssembleLocationRequest( Address address)
    {
        var apiUrl = $"{_locationIqConfig.BaseUrl}search?key={_locationIqConfig.ApiKey}&q={ConvertAddressToString(address)}&limit=1&format=json";

        return new HttpRequestMessage(HttpMethod.Get, apiUrl);
    }

    private static string ConvertAddressToString(Address address)
    {
        return address.Street + " " + address.StreetNumber + ", " + address.City + ", " + address.Country;
    }
}