using System;
using System.Collections.Generic;

namespace WeatherApi.Services.Models
{
    public class NasaWeatherResponse
    {
        public string? Type { get; set; }
        public Geometry? Geometry { get; set; }
        public Properties? Properties { get; set; }
        public Header? Header { get; set; }
        public List<string>? Messages { get; set; }
        public Dictionary<string, ParameterDetail>? Parameters { get; set; }
        public Times? Times { get; set; }
    }

    public class Geometry
    {
        public string? Type { get; set; }
        public List<double>? Coordinates { get; set; }
    }

    public class Properties
    {
        public ParameterValues? Parameter { get; set; }
    }

    public class ParameterValues
    {
        public Dictionary<string, Dictionary<string, double>>? Values { get; set; }
    }

    public class Header
    {
        public string? Title { get; set; }
        public ApiInfo? Api { get; set; }
        public List<string>? Sources { get; set; }
        public double Fill_Value { get; set; }
        public string? Time_Standard { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
    }

    public class ApiInfo
    {
        public string? Version { get; set; }
        public string? Name { get; set; }
    }

    public class ParameterDetail
    {
        public string? Units { get; set; }
        public string? LongName { get; set; }
    }

    public class Times
    {
        public double Data { get; set; }
        public double Process { get; set; }
    }
}