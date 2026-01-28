FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DeviceMonitoringApp.Api/DeviceMonitoringApp.Api.csproj", "DeviceMonitoringApp.Api/"]
RUN dotnet restore "DeviceMonitoringApp.Api/DeviceMonitoringApp.Api.csproj"

COPY . .
WORKDIR "/src/src/DeviceMonitoringApp.Api"
RUN dotnet build "DeviceMonitoringApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeviceMonitoringApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceMonitoringApp.Api.dll"]
