# XPike.Redis

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike-redis?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=6&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.Redis)

Provides basic Redis support for xPike.

## Overview

xPike Redis is the recommended caching provider for xPike.

It uses StackExchange.Redis, the leading .NET connection library for Redis.  
Multiple connections to different Redis servers/clusters is supported.
The library is fully thread-safe and friendly with all dependency injection scenarios.

Supports caching (IDatabase), publishing and subscribing to topics (ISubscriber), and direct
access to the underlying IConnectionMultiplexer.

Connections are defined and managed via application settings.

## Exported Services

### Singletons

- **`IRedisConnectionProvider`**  
  **=> `RedisConnectionProvider`**

## Usage

### Register the Provider

**In ASP.NET Core:**

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.UseXPikeDependencyInjection()
            .AddXPikeRedis();
}
```


### Define your Settings

**`appspettings.json`:**

```json
{
  "XPike": {
    "Redis": {
      "Connections": {
        "default": {
          "ConnectionString": "localhost:6379",
          "Enabled": "true"
        },
        "myOtherCache": {
          "ConnectionString": "contoso5.redis.cache.windows.net,ssl=true,password=..."
          "Enabled": "true"
        }
      }
    }
  }
}
```

## Dependencies

- `XPike.Logging`
  - `XPike.Settings`
    - `XPike.Configuration`
      - `XPike.IoC`
    - `XPike.IoC`

## Building and Testing

Building from source and running unit tests requires a Windows machine with:

* .Net Core 3.0 SDK
* .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/xpike-redis/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://github.com/xpike/documentation/blob/master/docfx_project/articles/contributing.md)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
