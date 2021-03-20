#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Garage.Service/Garage.Service.csproj", "Garage.Service/"]
COPY ["../Home.Core/Home.Core.Security/Home.Core.Security.csproj", "../Home.Core/Home.Core.Security/"]
COPY ["Garage.Repository/Garage.Repository.csproj", "Garage.Repository/"]
COPY ["../Home.Core/Home.Core.Gpio/Home.Core.Gpio.csproj", "../Home.Core/Home.Core.Gpio/"]
COPY ["Garage.Persistance/Garage.Persistance.csproj", "Garage.Persistance/"]
RUN dotnet restore "Garage.Service/Garage.Service.csproj"
COPY . .
WORKDIR "/src/Garage.Service"
RUN dotnet build "Garage.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Garage.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Garage.Service.dll"]
