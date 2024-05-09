FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim
ARG TARGETARCH
EXPOSE 8080/TCP
WORKDIR /app/

COPY $TARGETARCH ./

ENTRYPOINT ["./WeatherClient"]