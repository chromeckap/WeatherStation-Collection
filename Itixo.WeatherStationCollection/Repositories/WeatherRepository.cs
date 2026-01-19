using Dapper;
using Itixo.WeatherStationCollection.Models;
using Npgsql;

namespace Itixo.WeatherStationCollection.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _connectionString;
    
    public WeatherRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveAsync(WeatherData weatherData)
    {
        const string sql = """
                           INSERT INTO weather_collection (id, timestamp, is_station_online, json_data)
                           VALUES (@Id, @Timestamp, @IsStationOnline, @JsonData::jsonb)
                           """;
        
        await using var postgresConnection = new NpgsqlConnection(_connectionString);
        await postgresConnection.ExecuteAsync(sql, weatherData);
    }
}