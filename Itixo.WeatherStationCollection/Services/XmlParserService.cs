using System.Text.Json;
using System.Xml.Linq;
using Itixo.WeatherStationCollection.Models;

namespace Itixo.WeatherStationCollection.Services;

public class XmlParserService : IXmlParserService
{
    private static readonly JsonSerializerOptions JsonOptions = new() 
        { WriteIndented = true };
    
    public string ConvertToJson(string xml)
    {
        var parsedDocument = XDocument.Parse(xml);
        var rootElement = parsedDocument.Root
            ?? throw new InvalidOperationException("Root element not found in XML document.");

        var sensors = rootElement.Descendants("sensor")
            .Select(sensor => new SensorData
            {
                Type = sensor.Element("type")?.Value ?? string.Empty,
                Name = sensor.Element("name")?.Value ?? string.Empty,
                Value = sensor.Element("value")?.Value ?? string.Empty
            }).ToList();
        
        return JsonSerializer.Serialize(sensors, JsonOptions);
    }
}