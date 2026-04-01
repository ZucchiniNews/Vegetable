namespace Zucchinimvc.Infrastructure.ApiClients.OpenWeatherMapClient;

public interface IOpenWeatherMapClient
{
    Task<StrapiResponse<T>> GetAsync<T>(string endpoint);

}

