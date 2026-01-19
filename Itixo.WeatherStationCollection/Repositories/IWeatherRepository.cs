using Itixo.WeatherStationCollection.Models;

namespace Itixo.WeatherStationCollection.Repositories;

public interface IWeatherRepository
{
    Task SaveAsync(WeatherData weatherData);
}