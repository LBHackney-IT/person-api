FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telemetry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG PactBrokerUserPassword
ENV PactBrokerUserPassword=$PactBrokerUserPassword

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

WORKDIR /app

# Install and run sonar cloud scanner
RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet sonarscanner begin /k:"LBHackney-IT_person-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"

# Copy csproj and nuget config and restore as distinct layers
COPY ./PersonApi.sln ./
COPY ./PersonApi/PersonApi.csproj ./PersonApi/
COPY ./PersonApi.Tests/PersonApi.Tests.csproj ./PersonApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./PersonApi/PersonApi.csproj
RUN dotnet restore ./PersonApi.Tests/PersonApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out PersonApi.Tests/PersonApi.Tests.csproj

CMD dotnet test
