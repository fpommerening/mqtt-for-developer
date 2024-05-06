using FP.Mqtt.WeatherClient.Models;

namespace FP.Mqtt.WeatherClient.Business;

public interface ICrawlerRepository
{
    void AddData(WeatherData data);
    
     WeatherData? Latest { get; }
}