using System.Collections.Generic;
using PointOfSales.WebUI.Models;

namespace PointOfSales.WebUI.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
