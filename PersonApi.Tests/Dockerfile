FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./PersonApi.sln ./
COPY ./PersonApi/PersonApi.csproj ./PersonApi/
COPY ./PersonApi.Tests/PersonApi.Tests.csproj ./PersonApi.Tests/

RUN dotnet restore ./PersonApi/PersonApi.csproj
RUN dotnet restore ./PersonApi.Tests/PersonApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out PersonApi.Tests/PersonApi.Tests.csproj

CMD dotnet test
