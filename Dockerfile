FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY src/Campaign/Promo.Campaign.API/*.csproj ./
COPY src/Campaign/Promo.Campaign.Core/*.csproj ./
COPY src/Campaign/Promo.Campaign.Infrastructure/*.csproj ./
COPY src/Emails/*/*.csproj ./
COPY src/Harvester/*/*.csproj ./
COPY src/Membership/*/*.csproj ./
COPY src/Reporting/*/*.csproj ./
COPY src/Rules/*/*.csproj ./
COPY src/Sweeps/*/*.csproj ./
COPY src/Promo.Clients/*.csproj ./
COPY src/Promo.Core/*.csproj ./

RUN for f in ./*.csproj; do dotnet restore $f --source https://api.nuget.org/v3/index.json; done

COPY / /code
WORKDIR /code

#######################################################################################

FROM build as publish-campaignapi
RUN dotnet publish Promo.Campaign.API/Promo.Campaign.API.csproj -c Release -o /app

FROM build as publish-emailapi
RUN dotnet publish Promo.Emails.API/Promo.Emails.API.csproj -c Release -o /app

FROM build as publish-membershipapi
RUN dotnet publish Promo.Membership.API/Promo.Membership.API.csproj -c Release -o /app

FROM build as publish-reportinapi
RUN dotnet publish Promo.Reporting.API/Promo.Reporting.API.csproj -c Release -o /app

FROM build as publish-rulesapi
RUN dotnet publish Promo.Rules.API/Promo.Rules.API.csproj -c Release -o /app

FROM build as publish-sweepsapi
RUN dotnet publish Promo.Sweeps.API/Promo.Sweeps.API.csproj -c Release -o /app

#######################################################################################

FROM base as final-campaignapi
WORKDIR /app
COPY --from=publish-campaignapi /app .