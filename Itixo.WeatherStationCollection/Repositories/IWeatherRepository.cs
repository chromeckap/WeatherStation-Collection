using Itixo.WeatherStationCollection.Models;

namespace Itixo.WeatherStationCollection.Repositories;

public interface IWeatherRepository
{
    Task InitializeWeatherTableAsync();
    Task SaveAsync(WeatherData weatherData);
}