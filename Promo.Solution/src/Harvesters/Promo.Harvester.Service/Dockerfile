#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Harvesters/Promo.Harvester.Service/Promo.Harvester.Service.csproj", "Harvesters/Promo.Harvester.Service/"]
COPY ["Harvesters/Promo.Harvester.Core/Promo.Harvester.Core.csproj", "Harvesters/Promo.Harvester.Core/"]
COPY ["Harvesters/Promo.Harvester.Infrastructure/Promo.Harvester.Infrastructure.csproj", "Harvesters/Promo.Harvester.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
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
