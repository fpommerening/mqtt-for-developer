using FP.Mqtt.WeatherClient.Models;

namespace FP.Mqtt.WeatherClient.Business;

public interface IMqttRepository
{
    TimeSpan GetPublishInterval();
    void SetPublishInterval(int intervalInSeconds);
    void Add(string topic, string payload);
    IReadOnlyList<PublishData> GetAllData();
}