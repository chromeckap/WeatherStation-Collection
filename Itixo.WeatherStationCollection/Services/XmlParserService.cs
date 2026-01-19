using System.Globalization;
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
        var document = XDocument.Parse(xml);
        var root = document.Root
            ?? throw new InvalidOperationException("Root element not found in XML document.");

        var input = ParseSensors(root.Element("input"));
        var output = ParseSensors(root.Element("output"));
        var variables = ParseVariables(root.Element("variable"));
        var minMax = ParseMinMax(root.Element("minmax"));

        var weatherStationData = new WeatherStationDto(
            Input: input,
            Output: output,
            Variables: variables,
            MinMax: minMax
        );
        
        return JsonSerializer.Serialize(weatherStationData, JsonOptions);
    }

    private static List<SensorDto> ParseSensors(XElement? sensorElement)
    {
        if (sensorElement == null) return [];
        
        // Uses Elements() because data are inside separate elements (<tag>data</tag>)
        return sensorElement.Elements("sensor")
            .Select(sensor => new SensorDto(
                Type: sensor.Element("type")?.Value ?? string.Empty,
                Id: int.TryParse(sensor.Element("id")?.Value, out var id) ? id : null,
                Name: sensor.Element("name")?.Value ?? string.Empty,
                Place: sensor.Element("place")?.Value ?? string.Empty,
                Value: sensor.Element("value")?.Value ?? string.Empty
            ))
            .ToList();
    }

    private static Dictionary<string, string> ParseVariables(XElement? variableElement)
    {
        if (variableElement == null) return new Dictionary<string, string>();
        
        return variableElement.Elements()
            .ToDictionary(
                e => e.Name.LocalName,
                e => e.Value
            );
    }
    
    private static List<MinMaxDto> ParseMinMax(XElement? minMaxElement)
    {
        if (minMaxElement == null) return [];
        
        // Uses Attribute() because data are inside the tag, not separate elements (<tag/>)
        return minMaxElement.Elements("s")
            .Select(minMax => new MinMaxDto(
                Id: int.TryParse(minMax.Attribute("id")?.Value, out var id) ? id : null,
                Min: decimal.TryParse(minMax.Attribute("min")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var min) ? min : null,
                Max: decimal.TryParse(minMax.Attribute("max")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var max) ? max : null
            ))
            .ToList();
    }
}