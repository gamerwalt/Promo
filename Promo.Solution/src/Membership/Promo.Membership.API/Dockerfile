#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Membership/Promo.Membership.API/Promo.Membership.API.csproj", "Membership/Promo.Membership.API/"]
COPY ["Membership/Promo.Membership.Infrastructure/Promo.Membership.Infrastructure.csproj", "Membership/Promo.Membership.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
COPY ["Membership/Promo.Membership.Core/Promo.Membership.Core.csproj", "Membership/Promo.Membership.Core/"]
COPY ["Email/Promo.Email.Service/Promo.Email.Service.csproj", "Email/Promo.Email.Service/"]
COPY ["Email/Promo.Emails.Infrastructure/Promo.Emails.Infrastructure.csproj", "Email/Promo.Emails.Infrastructure/"]
RUN dotnet restore "Membership/Promo.Membership.API/Promo.Membership.API.csproj"
COPY . .
WORKDIR "/src/Membership/Promo.Membership.API"
RUN dotnet build "Promo.Membership.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Membership.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promo.Membership.API.dll"]
