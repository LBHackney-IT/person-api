# Person API

Person API stores and returns person records for Housing. Callers send a person id or a person payload. The API reads and writes those records in DynamoDB, allocates the next `personRef` from the `RefGenerator` table when a person is created, and publishes a person event to SNS when a record is created or actually changed.

Published API definition: [personAPI 1.0.0 on SwaggerHub](https://app.swaggerhub.com/apis/Hackney/personAPI/1.0.0).

## Endpoints

| Method and route | What it does |
| --- | --- |
| `GET /api/v1/persons/{id}` | Loads the person from the `Persons` table. Returns the record and an `ETag` of its version number. Returns 404 when the id is missing. `DISALLOWED_EMAIL` is blocked from one restricted person id. |
| `POST /api/v1/persons` | Creates a person, sets `personRef` to the next value in `RefGenerator`, and publishes `PersonCreatedEvent` version `v1`. |
| `PATCH /api/v1/persons/{id}` | Updates only the fields present in the body. Requires `If-Match` to match the stored version (409 on conflict). Publishes `PersonUpdatedEvent` version `v1` when something changed. Returns 204. |
| `POST /api/v2/persons` | Creates a person the same way as v1 and publishes `PersonCreatedEvent` version `v2`. |
| `GET /api/v1/healthcheck/ping` | DynamoDB health check. |
| `GET /swagger/{proxy+}` | Swagger UI and the generated OpenAPI documents. |

## Events published

Creates and updates publish to the topic in `PERSON_SNS_ARN`.

| Event | When |
| --- | --- |
| `PersonCreatedEvent` (`v1`) | After `POST /api/v1/persons`. Source system `PersonAPI`, source domain `Person`. |
| `PersonCreatedEvent` (`v2`) | After `POST /api/v2/persons`. Same source system and domain. |
| `PersonUpdatedEvent` (`v1`) | After `PATCH /api/v1/persons/{id}` when at least one field changed. The message carries the old and new values. |

## Environment variables

| Variable | Where it is set | Purpose |
| --- | --- | --- |
| `PERSON_SNS_ARN` | `ssm:/sns-topic/{stage}/person/arn` | Topic ARN for person events. |
| `ASPNETCORE_ENVIRONMENT` | `ssm:/housing-tl/{stage}/aspnetcore-environment` | ASP.NET Core environment name. |
| `DISALLOWED_EMAIL` | `ssm:/housing-tl/{stage}/disallowed-email` | Caller email blocked from one person record on get. |
| `DynamoDb_LocalMode` | `appsettings.json` (`false`); local and test config set `true` | Use a local DynamoDB endpoint instead of AWS. |
| `DynamoDb_LocalServiceUrl` | Local development `http://localhost:8000`; Docker `http://dynamodb-database:8000` | DynamoDB endpoint when local mode is on. |
| `Localstack_SnsServiceUrl` | Local development `http://localhost:4566`; Docker `http://localstack:4566` | LocalStack SNS endpoint. |
| `Sns_LocalMode` | Set to `true` by the test host | Publish to local SNS rather than AWS. |
| `AWS_REGION` | `eu-west-2` in `appsettings.json` and Docker Compose | AWS region. |
| `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY` | Docker Compose sets both to `local` | Credentials for DynamoDB Local and LocalStack. |

`serverless.yml` also creates an AppConfig application `housing-tl/{stage}/person-api`. Its configuration profile points at `ssm-parameter:///housing-tl/person-api/configurationData` and uses the role `ssm:/housing-tl/{stage}/app-config/retrieval-role-arn`. This repository does not show the API process reading that document.

## Stack

- .NET 10 (`net10.0`), hosted as an ASP.NET Core app on AWS Lambda.
- API Gateway (Lambda proxy and a public Swagger route), Lambda request authorizer, WAF web ACL `Platform_APIs_Web_ACL`, X-Ray, and a VPC security group. Region `eu-west-2`.
- SNS topic from `ssm:/sns-topic/{stage}/person/arn`.
- DynamoDB tables `Persons` (partition key `id`) and `RefGenerator` (partition key `refName`). Terraform manages both tables.

## Contacts

Housing product team: [mmh-project-team@hackney.gov.uk](mailto:mmh-project-team@hackney.gov.uk).

## Development

Build, run, and test steps are in [CONTRIBUTION.md](CONTRIBUTION.md).
