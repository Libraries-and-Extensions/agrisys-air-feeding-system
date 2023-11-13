FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AgrisysAirFeedingSystem/AgrisysAirFeedingSystem.csproj", "AgrisysAirFeedingSystem/"]
RUN dotnet restore "AgrisysAirFeedingSystem/AgrisysAirFeedingSystem.csproj"
COPY . .
WORKDIR "/src/AgrisysAirFeedingSystem"
RUN dotnet build "AgrisysAirFeedingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgrisysAirFeedingSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgrisysAirFeedingSystem.dll"]
