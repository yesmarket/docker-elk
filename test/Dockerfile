FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5045
LABEL description="LogstashTest"

FROM microsoft/dotnet:2.1-sdk AS build
COPY . ./
WORKDIR LogstashTest
RUN dotnet build -c Debug -o /app

# PUBLISH STAGE
FROM build AS publish
RUN dotnet publish -c Release -o /app

# FINAL STAGE
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LogstashTest.dll"]
