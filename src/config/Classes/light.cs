public class lightSensor
{
    private ColorSensor sensor;

    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    public double raw
    {
        get => sensor.Analog.Brightness;
    }

    public double light
    {
        get => 100 - raw;
    }

    public double red
    {
        get => sensor.Analog.Red;
    }

    public double green
    {
        get => sensor.Analog.Green;
    }

    public double blue
    {
        get => sensor.Analog.Blue;
    }

    public string color
    {
        get => sensor.Analog.ToString();
    }

    public bool isColorBlack
    {
        get => !sensor.Digital;
    }
};