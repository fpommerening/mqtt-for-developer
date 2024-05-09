using System.Globalization;
using FP.Mqtt.WeatherClient.Business;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace FP.Mqtt.WeatherClient.Services;

public class MqttClientService(
    ICrawlerRepository crawlerRepository,
    IMqttRepository mqttRepository,
    IConfiguration configuration,
    ILogger<MqttClientService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mqttSettings = configuration.GetSection("mqtt").Get<Models.MqttSettings>();
        if (mqttSettings == null)
        {
            logger.LogError("Unable to read mqtt server settings");
            return;
        }

        var clientOptions = new MqttClientOptionsBuilder()
            .WithClientId($"weather-{Guid.NewGuid().ToString("N").Substring(0, 10)}");

        switch (mqttSettings.Protocol.ToLowerInvariant())
        {
            case "mqtt":
            case "mqtts":
                clientOptions.WithTcpServer(mqttSettings.Host, mqttSettings.Port);
                break;
            case "ws":
            case "wss":
                clientOptions.WithWebSocketServer(ws =>
                    ws.WithUri($"{mqttSettings.Host}:{mqttSettings.Port}/{mqttSettings.Path}"));
                break;
            default:
                logger.LogError($"protokol '{mqttSettings.Protocol}' is not supported");
                return;
        }

        if (mqttSettings.Protocol.Equals("mqtts", StringComparison.InvariantCultureIgnoreCase) || 
            mqttSettings.Protocol.Equals("wss", StringComparison.InvariantCultureIgnoreCase))
        {
            clientOptions.WithTlsOptions(tls => tls.UseTls());
        }

        if (!string.IsNullOrEmpty(mqttSettings.User) && !string.IsNullOrEmpty(mqttSettings.Password))
        {
            clientOptions.WithCredentials(mqttSettings.User, mqttSettings.Password);
        }
        
        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(clientOptions.Build())
            .Build();

        var location = configuration.GetSection("location").Get<Models.Location>();
        var locationName = string.IsNullOrEmpty(location?.Name) ? "none" : location.Name.ToLowerInvariant();

        string temperatureTopic = $"weather/global/{locationName}/temperature/";
        string humidityTopic = $"weather/global/{locationName}/humidity/";

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var mqttClient = new MqttFactory().CreateManagedMqttClient();
                await mqttClient.StartAsync(options);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var currentValue = crawlerRepository.Latest;
                    if (currentValue != null)
                    {
                        await EnqueueAndLogAsync(mqttClient, temperatureTopic,
                            currentValue.Temperature.ToString("F", CultureInfo.InvariantCulture));
                        await EnqueueAndLogAsync(mqttClient,humidityTopic, currentValue.Humidity.ToString());
                    }
                    await Task.Delay(mqttRepository.GetPublishInterval(), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error on query open-metro");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

    private async Task EnqueueAndLogAsync(IManagedMqttClient client, string topic, string payload)
    {
        await client.EnqueueAsync(topic, payload);
        mqttRepository.Add(topic, payload);
    }
}