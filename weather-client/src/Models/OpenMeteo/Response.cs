using System.Text.Json.Serialization;

namespace FP.Mqtt.WeatherClient.Models.OpenMeteo;

public record Response(
    [property: JsonPropertyName("latitude")]
    double Latitude,
    [property: JsonPropertyName("longitude")]
    double Longitude,
    [property: JsonPropertyName("generationtime_ms")]
    double GenerationTimeMs,
    [property: JsonPropertyName("utc_offset_seconds")]
    int UtcOffsetSeconds,
    [property: JsonPropertyName("timezone")]
    string Timezone,
    [property: JsonPropertyName("timezone_abbreviation")]
    string TimezoneAbbreviation,
    [property: JsonPropertyName("elevation")]
    double Elevation,
    [property: JsonPropertyName("current_units")]
    CurrentUnits CurrentUnits,
    [property: JsonPropertyName("current")]
    Current Current
);