FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated10.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Itixo.WeatherStationCollection/Itixo.WeatherStationCollection.csproj", "Itixo.WeatherStationCollection/"]
RUN dotnet restore "Itixo.WeatherStationCollection/Itixo.WeatherStationCollection.csproj"
COPY . .
WORKDIR "/src/Itixo.WeatherStationCollection"
RUN dotnet build "./Itixo.WeatherStationCollection.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Itixo.WeatherStationCollection.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
