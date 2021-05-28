FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ENV DynamoDb_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./PersonApi/PersonApi.csproj ./
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./PersonApi.csproj

# Copy everything else and build
COPY . ./
RUN dotnet build -c Release -o out ./PersonApi.csproj
RUN dotnet publish -c Release -o out ./PersonApi.csproj

EXPOSE ${PORT:-3000}
CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/PersonApi.dll
