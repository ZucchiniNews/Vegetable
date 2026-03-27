using Azure.Data.Tables;
using Newtonsoft.Json;
using Zucchinimvc.Models.Entities;
using Zucchinimvc.Models.ViewModels;
using Zucchinimvc.Models.Weather;
using Zucchinimvc.Repositories;

namespace Zucchinimvc.Services.API
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IHistoryRepository<WeatherHistoryEntity> _repository;
        private readonly TableClient? _tableClient;

        public WeatherService(
            HttpClient httpClient,
            IConfiguration config,
            IHistoryRepository<WeatherHistoryEntity> repository,
            TableClient? tableClient = null)
        {
            _httpClient = httpClient;
            _repository = repository;
            _tableClient = tableClient;
            _apiKey = config["WeatherApi:ApiKey"] ?? string.Empty;
        }

        private bool HasValidApiKey()
        {
            return !string.IsNullOrWhiteSpace(_apiKey);
        }

        public async Task<WeatherViewModel?> GetWeatherByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return null;

            if (!HasValidApiKey())
                return null;

            var location = await GetCoordinatesAsync(city);

            if (location == null)
            {
                return new WeatherViewModel
                {
                    City = city
                };
            }

            var weather = await GetWeatherAsync(location.Lat, location.Lon);

            if (weather?.Main == null)
                return null;

            var entity = new WeatherHistoryEntity
            {
                PartitionKey = city,
                RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"),
                Temperature = weather.Main.Temp,
                Humidity = weather.Main.Humidity,
                Condition = weather.Weather?.FirstOrDefault()?.Description ?? "",
                RecordedAt = DateTime.UtcNow
            };

            await _repository.UpsertDailyAsync(entity);

            return new WeatherViewModel
            {
                City = city,
                Temp = weather.Main.Temp,
                Description = weather.Weather?.FirstOrDefault()?.Description ?? "",
                Icon = weather.Weather?.FirstOrDefault()?.Icon ?? ""
            };
        }

        public async Task<GeoLocation?> GetCoordinatesAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || !HasValidApiKey())
                return null;

            string encodedCity = Uri.EscapeDataString(city);

            string url =
                $"http://api.openweathermap.org/geo/1.0/direct?q={encodedCity}&limit=1&appid={_apiKey}";

            var response = await _httpClient.GetStringAsync(url);

            if (string.IsNullOrWhiteSpace(response))
                return null;

            var data = JsonConvert.DeserializeObject<List<GeoLocation>>(response);

            return data?.FirstOrDefault();
        }

        public async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
        {
            if (!HasValidApiKey())
                return null;

            string url =
                $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";

            var response = await _httpClient.GetStringAsync(url);

            if (string.IsNullOrWhiteSpace(response))
                return null;

            return JsonConvert.DeserializeObject<WeatherResponse>(response);
        }

        public async Task<List<WeatherHistoryEntity>> GetWeatherAsync()
        {
            var results = new List<WeatherHistoryEntity>();

            if (_tableClient == null)
                return results;

            await foreach (var entity in _tableClient.QueryAsync<WeatherHistoryEntity>())
            {
                if (entity != null)
                    results.Add(entity);
            }

            return results.OrderBy(e => e.RecordedAt).ToList();
        }

        public async Task<List<WeatherHistoryEntity>> GetHistoryAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return new List<WeatherHistoryEntity>();

            var data = await _repository.GetRecentByPartitionKeyAsync(city, 10);

            return data?
                .Where(x => x != null)
                .OrderBy(x => x.RecordedAt)
                .ToList()
                ?? new List<WeatherHistoryEntity>();
        }
    }
}