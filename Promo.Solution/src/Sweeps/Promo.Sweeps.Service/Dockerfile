#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Sweeps/Promo.Sweeps.Service/Promo.Sweeps.Draw.Service.csproj", "Sweeps/Promo.Sweeps.Service/"]
COPY ["Sweeps/Promo.Sweeps.Core/Promo.Sweeps.Core.csproj", "Sweeps/Promo.Sweeps.Core/"]
COPY ["Sweeps/Promo.Sweeps.Infrastructure/Promo.Sweeps.Infrastructure.csproj", "Sweeps/Promo.Sweeps.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
RUN dotnet restore "Sweeps/Promo.Sweeps.Service/Promo.Sweeps.Draw.Service.csproj"
COPY . .
WORKDIR "/src/Sweeps/Promo.Sweeps.Service"
RUN dotnet build "Promo.Sweeps.Draw.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Sweeps.Draw.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Sweeps.Draw.Service.dll"]