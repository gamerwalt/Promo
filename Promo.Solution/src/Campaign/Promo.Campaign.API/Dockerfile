#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Campaign/Promo.Campaign.API/Promo.Campaign.API.csproj", "Campaign/Promo.Campaign.API/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Campaign/Promo.Campaign.Core/Promo.Campaign.Core.csproj", "Campaign/Promo.Campaign.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
COPY ["Campaign/Promo.Campaign.Infrastructure/Promo.Campaign.Infrastructure.csproj", "Campaign/Promo.Campaign.Infrastructure/"]
RUN dotnet restore "Campaign/Promo.Campaign.API/Promo.Campaign.API.csproj"
COPY . .
WORKDIR "/src/Campaign/Promo.Campaign.API"
RUN dotnet build "Promo.Campaign.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Campaign.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Campaign.API.dll"]
