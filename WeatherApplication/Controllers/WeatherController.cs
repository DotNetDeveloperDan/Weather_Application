using Microsoft.AspNetCore.Mvc;
using WeatherApplication.Data;
using WeatherApplication.Models;
using WeatherApplication.Services.Implementations;

namespace WeatherApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(WeatherService weatherService, ApplicationDbContext context, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _context = context;
            _logger = logger;
        }

        [HttpGet("city/{id}")]
        public async Task<ActionResult<WeatherData>> GetWeather(int id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound();
                }

                var weatherData = await _weatherService.GetWeatherForCity(city.Latitude, city.Longitude);
                return weatherData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "There was a problem with the request. An error occurred." });
            }
        }
    }
}
