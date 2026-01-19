using Itixo.WeatherStationCollection.Models;
using Itixo.WeatherStationCollection.Repositories;
using Microsoft.Extensions.Logging;

namespace Itixo.WeatherStationCollection.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWeatherRepository _weatherRepository;
    private readonly IXmlParserService _xmlParserService;
    private readonly string _weatherStationUrl;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(
        IHttpClientFactory httpClientFactory,
        IWeatherRepository weatherRepository, 
        IXmlParserService xmlParserService, 
        string weatherStationUrl, ILogger<WeatherService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _weatherRepository = weatherRepository;
        _xmlParserService = xmlParserService;
        _weatherStationUrl = weatherStationUrl;
        _logger = logger;
    }

    public async Task FetchAndProcessAsync()
    {
        var weatherData = await FetchWeatherDataAsync();
        await _weatherRepository.SaveAsync(weatherData);
    }
    
    private async Task<WeatherData> FetchWeatherDataAsync()
    {
        try
        {
            var xmlResponse = await FetchXmlFromStationAsync();

            return string.IsNullOrWhiteSpace(xmlResponse)
                ? CreateOfflineWeatherData()
                : new WeatherData { JsonData = _xmlParserService.ConvertToJson(xmlResponse) };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch data from weather station at {Url}", _weatherStationUrl);
            return CreateOfflineWeatherData();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during weather data fetch");
            return CreateOfflineWeatherData();
        }
    }

    private async Task<string> FetchXmlFromStationAsync()
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetStringAsync(_weatherStationUrl);
    }
    
    private static WeatherData CreateOfflineWeatherData() => new()
    {
        IsStationOnline = false
    };
}