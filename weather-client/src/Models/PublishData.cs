namespace FP.Mqtt.WeatherClient.Models;

public record PublishData(string Topic, string Payload)
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

    public Guid Id { get; } = Guid.NewGuid();
}
