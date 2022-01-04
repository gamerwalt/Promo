FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Harvesters/Promo.Harvester.Core/Promo.Harvester.Core.csproj", "Harvesters/Promo.Harvester.Core/"]
COPY ["Harvesters/Promo.Harvester.Infrastructure/Promo.Harvester.Infrastructure.csproj", "Harvesters/Promo.Harvester.Infrastructure/"]
COPY ["Harvesters/Promo.Harvester.Service/Promo.Harvester.Service.csproj", "Harvesters/Promo.Harvester.Service/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]

RUN dotnet restore "Harvesters/Promo.Harvester.Service/Promo.Harvester.Service.csproj"
COPY . .
WORKDIR "/src/Harvesters/Promo.Harvester.Service"
RUN dotnet build "Promo.Harvester.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Harvester.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Harvester.Service.dll"]