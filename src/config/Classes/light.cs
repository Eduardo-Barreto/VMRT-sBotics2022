/// <summary>
/// Gerencia um sensor de luz
/// </summary>

public class lightSensor
{
    /// <summary>
    /// Sensor de luz a ser gerenciado.
    /// </summary>
    private ColorSensor sensor;

    /// <summary>
    /// Construtor da classe
    /// </summary>
    /// <param name="sensorName">Nome do sensor</param>
    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    /// <summary>
    /// Intensidade da luz refletida pelo sensor (0 ~ 100%).
    /// </summary>
    public double light
    {
        get => sensor.Analog.Brightness/2.55f;
    }

    /// <summary>
    /// Intensidade do vermelho refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double red
    {
        get => sensor.Analog.Red;
    }

    /// <summary>
    /// Intensidade do verde refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double green
    {
        get => sensor.Analog.Green;
    }

    /// <summary>
    /// Intensidade do azul refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double blue
    {
        get => sensor.Analog.Blue;
    }

    /// <summary>
    /// Cor mais próxima identificada pelo sensor.
    /// </summary>
    public string color
    {
        get => sensor.Analog.ToString();
    }

    /// <summary>
    /// Indica se preto é a cor mais próxima identificada pelo sensor.
    /// </summary>
    public bool isColorBlack
    {
        get => !sensor.Digital;
    }
}