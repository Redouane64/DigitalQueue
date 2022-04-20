FROM mcr.microsoft.com/dotnet/sdk:6.0

ARG PORT

WORKDIR /app
COPY src/DigitalQueue.Web/*.csproj .
RUN dotnet restore

COPY .config .
RUN dotnet tool restore

COPY src/DigitalQueue.Web/ .
RUN dotnet libman restore

ENTRYPOINT ["dotnet", "run", "-c", "Release", "urls=\"http://*:$PORT\""]
