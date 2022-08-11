/**
* Gerencia um sensor de luz
*/

public class lightSensor
{
    private ColorSensor sensor; // Sensor de luz a ser gerenciado

    /**
     * @brief Construtor da classe
     *
     *
     * @param sensorName: (string) Sensor de luz a ser gerenciado
     */
    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    /**
     * @brief Retorna o valor do sensor de luz
     *
     *
     * @return (double) Valor do sensor de luz (0~100%)
     */
    public double light
    {
        get => sensor.Analog.Brightness/2.55f;
    }

    /**
     * @brief Retorna a intensidade do vermelho refletido pelo sensor
     *
     *
     * @return (double) Intensidade do vermelho refletido pelo sensor (0~255)
     */
    public double red
    {
        get => sensor.Analog.Red;
    }

    /**
     * @brief Retorna a intensidade do verde refletido pelo sensor
     *
     *
     * @return (double) Intensidade do verde refletido pelo sensor (0~255)
     */
    public double green
    {
        get => sensor.Analog.Green;
    }

    /**
     * @brief Retorna a intensidade do azul refletido pelo sensor
     *
     *
     * @return (double) Intensidade do azul refletido pelo sensor (0~255)
     */
    public double blue
    {
        get => sensor.Analog.Blue;
    }

    /**
    * @brief Retorna a cor mais próxima identificada pelo sensor
    */
    public string color
    {
        get => sensor.Analog.ToString();
    }

    /**
    * @brief Indica se preto é a cor mais próxima identificada pelo sensor
    */
    public bool isColorBlack
    {
        get => !sensor.Digital;
    }
}