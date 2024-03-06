
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

### Setup

1. **Docker Setup:**
   - Make sure Docker is installed on your system.
   - Use `docker-compose` to start the PostgreSQL, MongoDB, and Cassandra services.

2. **Application Setup:**
   - Clone the repository.
   - Restore the dependencies using `dotnet restore`.
   - Start the application using `dotnet run`.

### Executing the Program

- Navigate to the project's root directory.
- Run the application:
  ```bash
  dotnet run
  ```

## Help

For any issues or questions, refer to the project's issue tracker on GitHub.

## Authors

Your Name 
[@YourGitHub](https://github.com/YourGitHub)

## Version History

* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Thanks to all contributors who have helped with this project.
- References to any third-party libraries or frameworks used.
