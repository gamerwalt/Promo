#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Reporting/Promo.Reporting.API/Promo.Reporting.API.csproj", "Reporting/Promo.Reporting.API/"]
COPY ["Reporting/Promo.Reporting.Core/Promo.Reporting.Core.csproj", "Reporting/Promo.Reporting.Core/"]
COPY ["Reporting/Promo.Reporting.Infrastructure/Promo.Reporting.Infrastructure.csproj", "Reporting/Promo.Reporting.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
RUN dotnet restore "Reporting/Promo.Reporting.API/Promo.Reporting.API.csproj"
COPY . .
WORKDIR "/src/Reporting/Promo.Reporting.API"
RUN dotnet build "Promo.Reporting.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Reporting.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Campaign/Promo.Campaign.API/campaignapi.crt /usr/local/share/ca-certificates
RUN chmod 644 /usr/local/share/ca-certificates/campaignapi.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "Promo.Reporting.API.dll"]
