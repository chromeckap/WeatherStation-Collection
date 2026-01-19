namespace Itixo.WeatherStationCollection.Models;

public record SensorDto(
    string Type,
    int? Id,
    string Name,
    string Place,
    string Value
);