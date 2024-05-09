namespace FP.Mqtt.WeatherClient.Models;

public record PublishData(string Topic, string Payload, DateTimeOffset Timestamp );