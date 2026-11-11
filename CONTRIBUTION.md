# Contributing to Person API

## Prerequisites

- .NET 10 SDK
- Docker, for the Compose workflow in the Makefile
- `LBHPACKAGESTOKEN` set in the environment. `NuGet.Config` uses it to restore packages from `https://nuget.pkg.github.com/LBHackney-IT/index.json`. Docker Compose passes the same variable into the image build.

## Local dependencies

`docker-compose.yml` defines:

| Service | Image | Port |
| --- | --- | --- |
| `person-api` | Built from `PersonApi/Dockerfile` | `3000` |
| `person-api-test` | Built from `PersonApi.Tests/Dockerfile` | none published |
| `dynamodb-database` | `amazon/dynamodb-local` | `8000` |
| `localstack` | `localstack/localstack:0.14.1` (`SERVICES=sns,sqs`) | `4566` |

The API container is linked to DynamoDB Local and LocalStack. Its environment sets `DynamoDb_LocalMode=true`, `DynamoDb_LocalServiceUrl=http://dynamodb-database:8000`, `Localstack_SnsServiceUrl=http://localstack:4566`, `AWS_REGION=eu-west-2`, and local AWS credentials.

The test container uses those same endpoints and also sets `PactBrokerProviderName=Person API V1`, `PactBrokerPath=https://contract-testing-development.hackney.gov.uk/`, and `PactBrokerUser=pact-broker-user`. `PactBrokerUserPassword` and `SONAR_TOKEN` are build and run arguments.

The Visual Studio / `dotnet run` profile `person_api` listens on `http://localhost:5000` and opens `api/v1/healthcheck/ping`. It expects DynamoDB Local at `http://localhost:8000` and LocalStack at `http://localhost:4566`, with `ASPNETCORE_ENVIRONMENT=LocalDevelopment`.

`dotnet test` uses the same localhost URLs. The test host turns on `DynamoDb_LocalMode` and `Sns_LocalMode`.

## Build

```bash
make setup    # docker compose build
make build    # docker compose build person-api
```

## Run

```bash
make serve    # docker compose build person-api && docker compose up person-api
```

The API is then on port `3000`. `make shell` opens a shell in the API image.

## Test

```bash
make test
```

That target starts `test-database`, builds `person-api-test`, and runs it. `docker-compose.yml` names the database service `dynamodb-database`, not `test-database`. `make restart-db` also refers to a `test-database` image. To run the containers that are defined in Compose:

```bash
docker compose up -d dynamodb-database localstack
dotnet test
```

`dotnet test` from the repository root needs DynamoDB Local on port `8000` and LocalStack on port `4566`.

```bash
make lint
```

`make lint` installs or updates `dotnet-format` and runs `dotnet format`.

## Coverage

From `PersonApi.Tests`, with `dotnet-reportgenerator-globaltool` installed and `coverlet.msbuild` referenced by the test project:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
.\RunCoverage.bat
```

`RunCoverage.bat` collects Cobertura coverage into `TestResults\coverage.cobertura.xml`, writes an HTML report under `TestResults\CoverageReport`, and opens it.
