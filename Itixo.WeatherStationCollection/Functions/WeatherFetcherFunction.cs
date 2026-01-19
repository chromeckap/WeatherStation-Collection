using Itixo.WeatherStationCollection.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Itixo.WeatherStationCollection.Functions;

public class WeatherFetcherFunction
{
    private readonly ILogger<WeatherFetcherFunction> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherFetcherFunction(ILogger<WeatherFetcherFunction> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [Function("WeatherFetcherFunction")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
    {
        var executionTime = DateTime.UtcNow;
        
        try
        {
            await _weatherService.FetchAndProcessAsync();
            _logger.LogInformation("Weather data collection completed successfully at {Timestamp}", executionTime);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Critical error during weather data collection at {Timestamp}", executionTime);
        }
    }
}