namespace Itixo.WeatherStationCollection.Models;

public class WeatherData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsStationOnline { get; set; } = true;
    public string? JsonData { get; set; }
}