# Change Log

## [1.0.1]

Adjustments to NuGet package configuration.
Added `net452` as a target.

## [1.0.0]

Inital release.

Basic support for interacting with Redis servers / clusters:

- Manages simultaneous connections to different servers/clusters, isolated by name
- Connections are defined/managed via application settings
- Uses StackExchange.Redis, the leading Redis connection library for .NET
- Supports both caching (IDatabase) and publish/subscribe (ISubscriber)
- Allows direct access to the IConnectionMultiplexer
- Fully thread-safe and friendly with all dependency injection scenarios
