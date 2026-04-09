// using Zucchini.Domain.Entities;  // Shound NOT refer to Domain. need workaround
// using Zucchini.Presentation.Models.DTOs.WeatherDTOs; // Shound NOT refer to Presentaion. need workaround

namespace Zucchinimvc.Infrastructure.Repositories.WeatherRepo;

public interface IWeatherRepository
{
    Task<GeoLocation?> GetCoordinatesAsync(string city);
    Task<WeatherResponse?> GetWeatherAsync(double lat, double lon);
}
