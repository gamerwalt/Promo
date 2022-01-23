#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Harvesters/Promo.Harvester.Service/Promo.Harvester.Service.csproj", "Harvesters/Promo.Harvester.Service/"]
COPY ["Harvesters/Promo.Harvester.Core/Promo.Harvester.Core.csproj", "Harvesters/Promo.Harvester.Core/"]
COPY ["Harvesters/Promo.Harvester.Infrastructure/Promo.Harvester.Infrastructure.csproj", "Harvesters/Promo.Harvester.Infrastructure/"]
COPY ["Promo.Core/Promo.Core.csproj", "Promo.Core/"]
COPY ["Promo.Clients/Promo.Clients.csproj", "Promo.Clients/"]
RUN dotnet restore "Harvesters/Promo.Harvester.Service/Promo.Harvester.Service.csproj"
COPY . .
WORKDIR "/src/Harvesters/Promo.Harvester.Service"
RUN dotnet build "Promo.Harvester.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promo.Harvester.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Promo.Harvester.Service.dll"]

RUN apt-get update --qq && apt-get -y install cron -qq --force=yes

COPY *.sh ./
COPY schedule /etc/cron.d/schedule
RUN sed -i 's/\r//' export_env.sh \
    && sed -i 's/\r//' run_app.sh \
    && sed -i 's/\r//' /etc/cron.d/schedule \
    && chmod +x export_env.sh run_app.sh \
    && chmod 0644 /etc/cron.d/schedule

# Create log file
RUN touch /var/log/cron.log
RUN chmod 0666 /var/log/cron.log

# Volume required for tail command
VOLUME /var/log

# Run Cron
CMD /app/export_env.sh && /usr/sbin/cron && tail -f /var/log/cron.log