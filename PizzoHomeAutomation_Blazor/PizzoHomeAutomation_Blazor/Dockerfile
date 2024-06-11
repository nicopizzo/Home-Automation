FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-bookworm-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update -y
RUN apt-get install -y python3
RUN dotnet workload install wasm-tools
WORKDIR /src
COPY ["PizzoHomeAutomation_Blazor/PizzoHomeAutomation_Blazor.csproj", "PizzoHomeAutomation_Blazor/"]
COPY ["PizzoHomeAutomation_Blazor.Client/PizzoHomeAutomation_Blazor.Client.csproj", "PizzoHomeAutomation_Blazor.Client/"]
RUN dotnet restore "PizzoHomeAutomation_Blazor/PizzoHomeAutomation_Blazor.csproj"
COPY . .
WORKDIR "/src/PizzoHomeAutomation_Blazor"
RUN dotnet build "PizzoHomeAutomation_Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PizzoHomeAutomation_Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PizzoHomeAutomation_Blazor.dll"]