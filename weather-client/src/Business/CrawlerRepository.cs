using FP.Mqtt.WeatherClient.Models;

namespace FP.Mqtt.WeatherClient.Business;

public class CrawlerRepository : ICrawlerRepository
{
    private List<WeatherData> backingList = new List<WeatherData>();

    public WeatherData? Latest => backingList.LastOrDefault();

    public IReadOnlyList<WeatherData> GetAllData()
    {
        return backingList.AsReadOnly();
    }

    public void AddData(WeatherData data)
    {
        backingList.Add(data);   
    }
    
    
}