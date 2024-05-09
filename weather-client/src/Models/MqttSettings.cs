namespace FP.Mqtt.WeatherClient.Models;

public record MqttSettings(
    string Host,
    int Port,
    string Protocol,
    string Path,
    string User,
    string Password
    );