FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ./site/*.csproj .
RUN dotnet restore *.csproj
ENV ASPNETCORE_ENVIRONMENT=Production
COPY ./site .
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/out .
COPY ./site/Data/data.csv /app/Data/data.csv
ENTRYPOINT ["dotnet", "Site.dll"]