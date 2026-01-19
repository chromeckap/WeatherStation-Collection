namespace Itixo.WeatherStationCollection.Models;

public record WeatherStationDto(
    List<SensorDto> Input,
    List<SensorDto> Output,
    Dictionary<string, string> Variables,
    List<MinMaxDto> MinMax
);