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
    public Colors color
    {
        get => sensor.Analog.Closest();
    }

    /**
    * @brief Indica se preto é a cor mais próxima identificada pelo sensor
    */
    public bool isColorBlack
    {
        get => !sensor.Digital;
    }

    public bool isGreen
    {
        get => green > red + 15 && green > blue + 15;
    }

    /* metodo antigo
    bool verde(byte sensor)
    {
        float val_vermelho = bot.ReturnRed(sensor);
        float val_verde = bot.ReturnGreen(sensor);
        float val_azul = bot.ReturnBlue(sensor);
        byte media_vermelho = 13, media_verde = 82, media_azul = 4;
        int RGB = (int)(val_vermelho + val_verde + val_azul);
        sbyte vermelho = (sbyte)(map(val_vermelho, 0, RGB, 0, 100));
        sbyte verde = (sbyte)(map(val_verde, 0, RGB, 0, 100));
        sbyte azul = (sbyte)(map(val_azul, 0, RGB, 0, 100));
        return ((proximo(vermelho, media_vermelho, 2) && proximo(verde, media_verde, 2) && proximo(azul, media_azul, 2)) || cor(sensor) == "VERDE");
    }
    */
}