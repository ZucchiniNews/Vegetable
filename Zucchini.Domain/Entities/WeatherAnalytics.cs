using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;
public class WeatherAnalytics
{
    public Dictionary<string, List<WeatherHistory>> CityHistories { get; set; } = new();
}