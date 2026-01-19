using Itixo.WeatherStationCollection.Models;

namespace Itixo.WeatherStationCollection.Services;

public interface IWeatherService
{
    Task FetchAndProcessAsync();
}