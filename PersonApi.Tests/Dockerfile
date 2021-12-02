FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telemetry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG PactBrokerUserPassword
ENV PactBrokerUserPassword=$PactBrokerUserPassword

WORKDIR /app

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
