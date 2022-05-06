using System;

namespace Avalonia1.Models;

public class SensorData
{
    private string _sensorId = String.Empty;
    private string _sensorData = String.Empty;
    private string _time = TimeOnly.MinValue.ToString();
    private string _date = DateTime.MinValue.ToString();

    public string sensorId
    {
        get => _sensorId;
        set => _sensorId = value;
    }

    public string sensorData
    {
        get => _sensorData;
        set => _sensorData = value;
    }

    public string time
    {
        get => _time;
        set => _time = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string date
    {
        get => _date;
        set => _date = value ?? throw new ArgumentNullException(nameof(value));
    }
}