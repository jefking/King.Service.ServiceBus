FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /app

# Copy and build
COPY ./King.Service.ServiceBus ./King.Service.ServiceBus
COPY ./King.Service.ServiceBus.Demo ./Demo

# Public Project
RUN dotnet publish Demo/King.Service.ServiceBus.Demo.csproj -c release

# Create Output Container Image
FROM microsoft/dotnet:runtime
WORKDIR /app

# Copy Demo
COPY --from=build-env /app/Demo/bin/release/netcoreapp2.2/publish/. .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.ServiceBus.Demo.dll"]