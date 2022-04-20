FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app
COPY src/DigitalQueue.Web/*.csproj .
RUN dotnet restore

COPY .config .
RUN dotnet tool restore

COPY src/DigitalQueue.Web/ .
RUN dotnet libman restore

ARG PORT
CMD dotnet run --urls "http://*:$PORT"
