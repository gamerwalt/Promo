#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Winners/Promo.Winners.API/Promo.Winners.API.csproj", "Winners/Promo.Winners.API/"]
COPY ["Winners/Promo.Winners.Core/Promo.Winners.Core.csproj", "Winners/Promo.Winners.Core/"]
COPY ["Winners/Promo.Winners.Infrastructure/Promo.Winners.Infrastructure.csproj", "Winners/Promo.Winners.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
RUN dotnet restore "Winners/Promo.Winners.API/Promo.Winners.API.csproj"
COPY . .
WORKDIR "/src/Winners/Promo.Winners.API"
RUN dotnet build "Promo.Winners.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Winners.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Winners.API.dll"]