using FluentResults;

namespace Search.Api.LocationIq;

public interface ILocationClient
{
    Task<Result<Location>> GetAddressLocation(Address address);
}
