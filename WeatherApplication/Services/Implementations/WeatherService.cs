using System.Net.Http.Headers;
using System.Text.Json;
using WeatherApplication.Models;

namespace WeatherApplication.Services.Implementations
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;




        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherApp/1.0 (your.email@example.com)");
        }

        public async Task<WeatherData> GetWeatherForCity(double latitude, double longitude)
        {
            var gridPoints = await GetGridPoints(latitude, longitude);
            //Temp and Wind speed
            var weatherData = await GetWeatherData(gridPoints.gridId, gridPoints.gridX, gridPoints.gridY);

            //humidity
            var stationId = await GetNearestStation(latitude, longitude);
            var humidity = await GetHumidity(stationId);

            return new WeatherData()
            {
                Humidity = humidity,
                Temperature = weatherData.Temperature,
                WindSpeed = weatherData.WindSpeed
            };
        }


        private async Task<(double Temperature,double WindSpeed)> GetWeatherData(string gridId,
            int gridX, int gridY)
        {


            var url = $"https://api.weather.gov/gridpoints/{gridId}/{gridX},{gridY}/forecast";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var periods = jsonDoc.RootElement
                .GetProperty("properties")
                .GetProperty("periods")
                .EnumerateArray();

            // Assuming you want the first period (e.g., current forecast)
            var firstPeriod = periods.First();
            var temperature = firstPeriod.GetProperty("temperature").GetDouble();
            var windSpeedText = firstPeriod.GetProperty("windSpeed").GetString();
            var windSpeed = double.Parse(windSpeedText.Split(' ')[0]); // Convert "15 mph" to 15

            // Fetch humidity - Note: This example assumes humidity is provided in the forecast data
            // You might need to adjust this based on the actual structure of the API response
            var detailedForecast = firstPeriod.GetProperty("detailedForecast").GetString();


            return (temperature, windSpeed);

        }

        private async Task<(string gridId, int gridX, int gridY)> GetGridPoints(double latitude, double longitude)
        {
            try
            {
                var url = $"https://api.weather.gov/points/{latitude},{longitude}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonString);
                var properties = jsonDoc.RootElement.GetProperty("properties");
                var gridId = properties.GetProperty("gridId").GetString();
                var gridX = properties.GetProperty("gridX").GetInt32();
                var gridY = properties.GetProperty("gridY").GetInt32();
                return (gridId, gridX, gridY);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private double ExtractHumidity(string detailedForecast)
        {
            // A simple extraction based on a known pattern in the detailed forecast string
            // Adjust this function as needed based on actual data format
            var humidityText = "Humidity: ";
            var startIndex = detailedForecast.IndexOf(humidityText) + humidityText.Length;
            var endIndex = detailedForecast.IndexOf('%', startIndex);
            var humidity = detailedForecast.Substring(startIndex, endIndex - startIndex);
            return double.Parse(humidity);
        }



        private async Task<string> GetNearestStation(double latitude, double longitude)
        {

            var url = $"https://api.weather.gov/points/{latitude},{longitude}/stations";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var stations = jsonDoc.RootElement
                .GetProperty("features")
                .EnumerateArray();

            var nearestStationId =
                stations.First().GetProperty("properties").GetProperty("stationIdentifier").GetString();
            return nearestStationId;

        }



        private async Task<double> GetHumidity(string stationId)
        {

            var url = $"https://api.weather.gov/stations/{stationId}/observations/latest";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var humidity = jsonDoc.RootElement
                .GetProperty("properties")
                .GetProperty("relativeHumidity")
                .GetProperty("value")
                .GetDouble();

            return humidity;
        }

    }
}