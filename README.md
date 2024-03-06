
# Intelligent Data Management Project

This project demonstrates the integration and management of multiple databases including PostgreSQL, MongoDB, and Cassandra, within a .NET application. 

## Description

This application is designed to showcase how to configure and connect to different types of databases within a single .NET environment. plus Kafka and zookeeper

## NOTE
This lunches the ASP-net.core into  production via the docker file!

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
This is important to note since you use these to connect to the `docker-compose.yml`
```  postgres:
    image: 'postgres:13.2'
    volumes:
      - 'db-data:/var/lib/postgresql/data'
    ports:
      - '5432:5432'
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: IKT453
      POSTGRES_DB: employee

  cassandra:
    image: 'cassandra:latest'
    ports:
      - '9042:9042'
    environment:
      CASSANDRA_CLUSTER_NAME: myCassandraCluster
    volumes:
      - 'cassandra-data:/var/lib/cassandra'

  mongodb:
    image: 'mongo:latest'
    ports:
      - '27017:27017'
    volumes:
      - 'mongodb-data:/data/db'
    environment:
      MONGO_INITDB_ROOT_USERNAME: mongoadmin
      MONGO_INITDB_ROOT_PASSWORD: secret
```

### Setup for development

1. **Dotnet setup**
  - Make sure you have install dotnet 5 runtime and sdk
  - `https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-5.0.17-windows-x64-installer?cid=getdotnetcore`
  - `https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-5.0.408-windows-x64-installer`
2. **Make your changes to the project inside the `./site` folder**
3. **re-create the dockerfile**
  - while you are in the main `intelligent_data_management` directory use the `docker build -t dotnet-site .`
4. **Tag and push to docker hub**
  - `docker tag dotnet-site lemoi18/ikt435:latest` 
  - `docker push lemoi18/ikt435:latest`
  - make the necessary changes to the dockerhub to match your setup. EG: create your own dockerhub repo, and change `lemoi18/ikt435` to `yourname/yourrepo` in both
5. **Change the `docker-compose.yml`** 
  - here you want to change the dotnet-site image to your own like so
    ```
      dotnet:
    depends_on:
      - postgres
      - cassandra
      - mongodb
    build: .
    image: `yourname/yourrepo`
    ports:
      - '5000:5000'
    restart: always
    environment:
      - 'ASPNETCORE_URLS=http://+:5000'
      - ASPNETCORE_ENVIRONMENT=Production
    ```
  - Use `docker-compose up -d` to start the PostgreSQL, MongoDB,Cassandra, Kafka, zookeeper services.
  - Also you should change the Kafka listener to your ip adress of you host machine
     ```
         environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: 'zookeeper:2181'
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://broker:9092,PLAINTEXT_HOST://<add ip here>:29092
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS: 0
    ```

    
6. **When you just want to start it up you can just do this**
   - Use `docker-compose up -d` to start the PostgreSQL, MongoDB,Cassandra, Kafka, zookeeper services.



