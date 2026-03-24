using Newtonsoft.Json;
using Zucchinimvc.Models.Weather;

namespace Zucchinimvc.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["WeatherApi:ApiKey"];
        }

        public async Task<WeatherResponse?> GetWeatherByCityAsync(string city)
        {
            var location = await GetCoordinatesAsync(city);

            if (location == null)
                return null;

            return await GetWeatherAsync(location.Lat, location.Lon);
        }
        public async Task<GeoLocation> GetCoordinatesAsync(string city)
        {

            string encodedCity = Uri.EscapeDataString(city);

            string url = $"http://api.openweathermap.org/geo/1.0/direct?q={encodedCity}&limit=1&appid=_apiKey";

            var response = await _httpClient.GetStringAsync(url);

            var data = JsonConvert.DeserializeObject<List<GeoLocation>>(response);

            return data?.FirstOrDefault();
        }

        public async Task<WeatherResponse> GetWeatherAsync(double lat, double lon)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=_apiKey&units=metric";

            var response = await _httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<WeatherResponse>(response);
        }


    }

}
