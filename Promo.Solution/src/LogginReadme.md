For Logging...
Serilog
Serilog.AspNetCore
Serilog.Extensions.Logging
Serilog.Settings.Configuration
Serilog.Sinks.Console
Serilog.Sinks.Loggly

appsettings.json
"Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "Host": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "Debug"
      }
    ],
    "Properties": {
      "Application": "Promo.Sweeps.DrawService"
    }
  }
  appsettings.env.json (dev token)
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Loggly",
        "Args": {
          "customerToken": "e091e241-3cfd-4dbe-b71f-dcf38ac05329",
          "tags": "DrawService, Promo.Sweeps.DrawService, DrawService-dev, environment-dev"
        }
      }
    ]
  }