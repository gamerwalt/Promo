#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Sweeps/Promo.Sweeps.API/Promo.Sweeps.API.csproj", "Sweeps/Promo.Sweeps.API/"]
COPY ["Sweeps/Promo.Sweeps.Infrastructure/Promo.Sweeps.Infrastructure.csproj", "Sweeps/Promo.Sweeps.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Sweeps/Promo.Sweeps.Core/Promo.Sweeps.Core.csproj", "Sweeps/Promo.Sweeps.Core/"]
RUN dotnet restore "Sweeps/Promo.Sweeps.API/Promo.Sweeps.API.csproj"
COPY . .
WORKDIR "/src/Sweeps/Promo.Sweeps.API"
RUN dotnet build "Promo.Sweeps.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Sweeps.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Campaign/Promo.Campaign.API/campaignapi.crt /usr/local/share/ca-certificates
RUN chmod 644 /usr/local/share/ca-certificates/campaignapi.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "Promo.Sweeps.API.dll"]
