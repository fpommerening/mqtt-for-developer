using FP.Mqtt.WeatherClient.Models;

namespace FP.Mqtt.WeatherClient.Business;

public class MqttRepository : IMqttRepository
{
    private TimeSpan _publishInterval = TimeSpan.FromSeconds(30);
    private List<PublishData> backingList = new List<PublishData>();
    public TimeSpan GetPublishInterval()
    {
        return _publishInterval;
    }

    public void SetPublishInterval(int intervalInSeconds)
    {
        if (intervalInSeconds is < 5 or > 60)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalInSeconds),
                "Interval must be between 5 and 60 seconds");
        }
        _publishInterval = TimeSpan.FromSeconds(intervalInSeconds);
    }

    public void Add(string topic, string payload)
    {
        backingList.Add(new PublishData(topic, payload, DateTimeOffset.Now));
    }
    
    public IReadOnlyList<PublishData> GetAllData()
    {
        return backingList.AsReadOnly();
    }

}