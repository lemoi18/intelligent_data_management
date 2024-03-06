# Intelligent Data Management Project

This project demonstrates the integration and management of multiple databases including PostgreSQL, MongoDB, and Cassandra, within a .NET application. 

## Description

This application is designed to showcase how to configure and connect to different types of databases within a single .NET environment. The project is ideal for educational purposes, particularly for students learning about database connectivity and data management in .NET.

## Getting Started

### Dependencies

- .NET 5 SDK
- Docker
- PostgreSQL, MongoDB, Cassandra (Docker images are used in this project)

### Configuration

The project uses an `appsettings.json` file for configuration. Here is the structure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Username=postgres;Password=IKT453;Database=postgres;",
    "MongoDbConnection": "mongodb://mongoadmin:secret@mongodb:27017",
    "CassandraConnection": "Contact Points=cassandra:9042;Default Keyspace=IKT453"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```
