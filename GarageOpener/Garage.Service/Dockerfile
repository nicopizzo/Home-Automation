FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-bookworm-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GarageOpener/Garage.Service/Garage.Service.csproj", "GarageOpener/Garage.Service/"]
COPY ["Home.Core/Home.Core.Security/Home.Core.Security.csproj", "Home.Core/Home.Core.Security/"]
COPY ["GarageOpener/Garage.Repository/Garage.Repository.csproj", "GarageOpener/Garage.Repository/"]
COPY ["Home.Core/Home.Core.Gpio/Home.Core.Gpio.csproj", "Home.Core/Home.Core.Gpio/"]
COPY ["GarageOpener/Garage.Persistance/Garage.Persistance.csproj", "GarageOpener/Garage.Persistance/"]
RUN dotnet restore "GarageOpener/Garage.Service/Garage.Service.csproj"
COPY . .
WORKDIR "/src/GarageOpener/Garage.Service"
RUN dotnet build "Garage.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Garage.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Garage.Service.dll"]
