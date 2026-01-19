# Weather Station Collection
Aplikace sbírá data z meteostanice pomocí Azure Function s využitím Timer Trigger (každou hodinu). Data ve formátu XML jsou stahována z URL adresy, převedena do strukturovaného JSON formátu a uložena do PostgreSQL databáze. Součástí záznamu v databázi je pole `is_station_online` určující, zda bylo možné získat data z dané URL. Použité technologie .NET 10 Azurite Function, PostgreSQL 16, Azurite a Docker.

# Deployment
- Před spuštěním aplikace je nutné mít
  - Docker https://www.docker.com/get-started/
  - Není nutné, ale je vhodné mít editor, např. VS Code, JetBrains Rider, ...
```
# 1. Klonování repozitáře (přes terminál, kde se má aplikace nacházet)
gh repo clone chromeckap/WeatherStation-Collection

# 2. Otevřete složku a vytvořte .env soubor, do kterého vložte obsah:

POSTGRES_DB=replace-with-db
POSTGRES_USER=replace-with-username
POSTGRES_PASSWORD=replace-with-password
WEATHER_STATION_URL=replace-with-url

# 3. V terminálu přejděte do složky projektu
cd weatherstation-collection

# 4. Spuštění pomocí Docker compose
docker compose up -d --build
```

Aplikaci je možné spustit i lokálně. Nutné je ovšem stáhnout PostgreSQL https://www.pgadmin.org/download/. Následně vytvořte soubor `local.settings.json` v adresáři aplikace, kde vložíte (nezapomeňte upravit jednotlivá pole)
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "WeatherStationUrl": "replace-with-url",
        "PostgresConnection": "Host=localhost;Port=5432;Database=replace-with-db;Username=replace-with-username;Password=replace-with-password"
    }
}
```
