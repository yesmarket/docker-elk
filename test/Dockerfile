ARG HTTP_PROXY
ARG HTTPS_PROXY

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5045
LABEL description="LogstashTest"

FROM microsoft/dotnet:2.1-sdk AS build
COPY . ./
ENV http_proxy=${HTTP_PROXY}
ENV https_proxy=${HTTPS_PROXY}
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
