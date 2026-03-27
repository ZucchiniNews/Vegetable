    using Azure.Data.Tables;
    using Newtonsoft.Json;
    using ZucchiniCore.Models.Entities;
    using ZucchiniCore.Models.ViewModels;
    using ZucchiniCore.Models.Weather;
    using Zucchinimvc.Repositories;

    namespace Zucchini.Core.Services;

    public class WeatherService // : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IHistoryRepository<WeatherHistoryEntity> _repository;

        public WeatherService(
            HttpClient httpClient,
            IConfiguration config,
            IHistoryRepository<WeatherHistoryEntity> repository)
        {
            _httpClient = httpClient;
            _apiKey = config["WeatherApi:ApiKey"]
                ?? throw new ArgumentNullException("Weather API key is missing");

            _repository = repository;
        }

        public async Task<WeatherViewModel?> GetWeatherByCityAsync(string city)
        {
            var location = await GetCoordinatesAsync(city);

            if (location == null)
            {
                return new WeatherViewModel
                {
                    City = city
                };
            }

            var weather = await GetWeatherAsync(location.Lat, location.Lon);

            if (weather == null)
                return null;

            var entity = new WeatherHistoryEntity
            {
                PartitionKey = city,
                RowKey = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"),
                Temperature = weather.Main?.Temp ?? 0,
                Humidity = weather.Main?.Humidity ?? 0,
                Condition = weather.Weather?.FirstOrDefault()?.Description ?? "",
                RecordedAt = DateTime.UtcNow
            };

            await _repository.UpsertDailyAsync(entity);

            return new WeatherViewModel
            {
                City = weather.Name ?? city,
                Temp = weather.Main?.Temp ?? 0,
                Description = weather.Weather?.FirstOrDefault()?.Description ?? "",
                Icon = weather.Weather?.FirstOrDefault()?.Icon ?? ""
            };
        }

        private async Task<GeoLocation?> GetCoordinatesAsync(string city)
        {
            var encodedCity = Uri.EscapeDataString(city);

            var url =
                $"http://api.openweathermap.org/geo/1.0/direct?q={encodedCity}&limit=1&appid={_apiKey}";

            var response = await _httpClient.GetStringAsync(url);

            var data = JsonConvert.DeserializeObject<List<GeoLocation>>(response);

            return data?.FirstOrDefault();
        }

        private async Task<WeatherResponse?> GetWeatherAsync(double lat, double lon)
        {
            var url =
                $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";

            var response = await _httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<WeatherResponse>(response);
        }

        public async Task<List<WeatherHistoryEntity>> GetHistoryAsync(string city)
        {
            var data = await _repository.GetRecentByPartitionKeyAsync(city, 10);

            return data
                .OrderBy(x => x.RecordedAt)
                .ToList();
        }
    }

