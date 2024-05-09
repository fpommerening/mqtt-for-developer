using System.Globalization;
using FP.Mqtt.WeatherClient.Business;
using FP.Mqtt.WeatherClient.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace FP.Mqtt.WeatherClient.Services;

public class WeatherCrawlerService(
    ICrawlerRepository crawlerRepository,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    ILogger<WeatherCrawlerService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var location = configuration.GetSection("location").Get<Models.Location>() ??
                       new Models.Location("None", 0m, 0m);
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        
        var queryParams = new Dictionary<string, string>()
        {
            { "current", "temperature_2m,relative_humidity_2m" },
            { "latitude", location.Latitude.ToString("F", CultureInfo.InvariantCulture)},
            { "longitude", location.Longitude.ToString("F", CultureInfo.InvariantCulture)},
        };
        
        var query = QueryHelpers.AddQueryString(string.Empty, queryParams!);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var httpclient = httpClientFactory.CreateClient("open-meteo");
                var response = await httpclient.GetAsync(query, stoppingToken);
                var json = await response.Content.ReadFromJsonAsync<Models.OpenMeteo.Response>(stoppingToken);

                if (json?.Current != null)
                {
                    crawlerRepository.AddData(new WeatherData(json.Current.Temperature2m,
                        json.Current.RelativeHumidity2m, new DateTimeOffset(json.Current.Time, TimeSpan.Zero)));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error on query open-metro");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}