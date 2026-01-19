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
    
    public async Task InitializeWeatherTableAsync()
    {
        await using var postgresConnection = new NpgsqlConnection(_connectionString);
        await postgresConnection.OpenAsync();

        const string createWeatherTableSql = """
                                      CREATE TABLE IF NOT EXISTS weather_collection (
                                          id UUID PRIMARY KEY,
                                          timestamp TIMESTAMP WITH TIME ZONE NOT NULL,
                                          is_station_online BOOLEAN NOT NULL DEFAULT true,
                                          json_data JSONB
                                      );
                                      """;

        await using var command = new NpgsqlCommand(createWeatherTableSql, postgresConnection);
        await command.ExecuteNonQueryAsync();
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