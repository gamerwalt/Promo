#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Email/Promo.Emails.API/Promo.Emails.API.csproj", "Email/Promo.Emails.API/"]
COPY ["Email/Promo.Emails.Core/Promo.Emails.Core.csproj", "Email/Promo.Emails.Core/"]
COPY ["Email/Promo.Emails.Infrastructure/Promo.Emails.Infrastructure.csproj", "Email/Promo.Emails.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
RUN dotnet restore "Email/Promo.Emails.API/Promo.Emails.API.csproj"
COPY . .
WORKDIR "/src/Email/Promo.Emails.API"
RUN dotnet build "Promo.Emails.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Emails.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Campaign/Promo.Campaign.API/campaignapi.crt /usr/local/share/ca-certificates
RUN chmod 644 /usr/local/share/ca-certificates/campaignapi.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "Promo.Emails.API.dll"]