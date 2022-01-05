#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Rules/Promo.Rules.API/Promo.Rules.API.csproj", "Rules/Promo.Rules.API/"]
COPY ["Rules/Promo.Rules.Core/Promo.Rules.Core.csproj", "Rules/Promo.Rules.Core/"]
COPY ["Rules/Promo.Rules.Infrastructure/Promo.Rules.Infrastructure.csproj", "Rules/Promo.Rules.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
RUN dotnet restore "Rules/Promo.Rules.API/Promo.Rules.API.csproj"
COPY . .
WORKDIR "/src/Rules/Promo.Rules.API"
RUN dotnet build "Promo.Rules.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Rules.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Campaign/Promo.Campaign.API/campaignapi.crt /usr/local/share/ca-certificates
RUN chmod 644 /usr/local/share/ca-certificates/campaignapi.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "Promo.Rules.API.dll"]
