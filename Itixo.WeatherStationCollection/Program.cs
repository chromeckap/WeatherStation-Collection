using Itixo.WeatherStationCollection.Repositories;
using Itixo.WeatherStationCollection.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddHttpClient();

builder.Services.AddSingleton<IXmlParserService, XmlParserService>();

var postgresConnection = Environment.GetEnvironmentVariable("PostgresConnection") 
                         ?? throw new Exception("PostgresConnection string not found in environment variables.");

builder.Services.AddSingleton<IWeatherRepository>(new WeatherRepository(postgresConnection));

builder.Services.AddSingleton<IWeatherService>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var weatherRepository = serviceProvider.GetRequiredService<IWeatherRepository>();
    var xmlParserService = serviceProvider.GetRequiredService<IXmlParserService>();
    var logger = serviceProvider.GetRequiredService<ILogger<WeatherService>>();
    
    var weatherStationUrl = Environment.GetEnvironmentVariable("WeatherStationUrl") 
                            ?? throw new Exception("WeatherStationUrl not found in environment variables.");

    return new WeatherService(httpClientFactory, weatherRepository, xmlParserService, weatherStationUrl, logger);
});

builder.Build().Run();