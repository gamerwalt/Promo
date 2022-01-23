#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Prizes/Promo.Prizes.API/Promo.Prizes.API.csproj", "Prizes/Promo.Prizes.API/"]
COPY ["Prizes/Promo.Prizes.Core/Promo.Prizes.Core.csproj", "Prizes/Promo.Prizes.Core/"]
COPY ["Prizes/Promo.Prizes.Infrastructure/Promo.Prizes.Infrastructure.csproj", "Prizes/Promo.Prizes.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
RUN dotnet restore "Prizes/Promo.Prizes.API/Promo.Prizes.API.csproj"
COPY . .
WORKDIR "/src/Prizes/Promo.Prizes.API"
RUN dotnet build "Promo.Prizes.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Prizes.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Prizes.API.dll"]