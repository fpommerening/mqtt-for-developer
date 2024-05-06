namespace FP.Mqtt.WeatherClient.Models;

public record WeatherData(decimal Temperature, int Humidity, DateTimeOffset Timestamp);
