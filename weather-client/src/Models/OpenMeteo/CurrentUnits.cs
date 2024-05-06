using System.Text.Json.Serialization;

namespace FP.Mqtt.WeatherClient.Models.OpenMeteo;

public record CurrentUnits(
    [property: JsonPropertyName("time")]
    string Time,
    string Interval,
    [property: JsonPropertyName("temperature_2m")]
    string Temperature2m,
    [property: JsonPropertyName("relative_humidity_2m")]
    string RelativeHumidity2m
);