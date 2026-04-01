namespace Zucchinimvc.Infrastrcture.ApiClients.StrapiClient;

public interface IStrapiClient
{
    Task<StrapiResponse<T>> GetAsync<T>(string endpoint);
}