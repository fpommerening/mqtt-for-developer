using System.Text.Json.Serialization;

namespace FP.Mqtt.WeatherClient.Models.OpenMeteo;

public record Current(
    [property: JsonPropertyName("time")] 
    DateTime Time,
    [property: JsonPropertyName("interval")]
    int Interval,
    [property: JsonPropertyName("temperature_2m")]
    decimal Temperature2m,
    [property: JsonPropertyName("relative_humidity_2m")]
    int RelativeHumidity2m
);
    
    