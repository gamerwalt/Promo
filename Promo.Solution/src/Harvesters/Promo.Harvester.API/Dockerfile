#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Harvesters/Promo.Harvester.API/Promo.Harvester.API.csproj", "Harvesters/Promo.Harvester.API/"]
RUN dotnet restore "Harvesters/Promo.Harvester.API/Promo.Harvester.API.csproj"
COPY . .
WORKDIR "/src/Harvesters/Promo.Harvester.API"
RUN dotnet build "Promo.Harvester.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Harvester.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Harvester.API.dll"]