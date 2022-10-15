/**
* @brief Gerencia um led
*/

public class led
{
    /**
    * @brief Led a ser gerenciado.
    */
    private Light ledLight;
    private long lastBlink;

    /**
    * @brief Construtor da classe.
    *
    * @param ledName: Nome do led 
    */
    public led(string ledName)
    {
        this.ledLight = Bot.GetComponent<Light>(ledName);
    }

    /**
    * @brief Indica se o led está ligado.
    *
    */
    public bool state
    {
        get => ledLight.Lit;
    }

    /**
    * @brief Indica a cor atual do led;
    *
    */
    public Color color
    {
        get => ledLight.Color;
    }

    /**
    * @brief Liga o led com a cor especificada.
    *
    * @param color: Cor desejada (padrão vermelho)
    */
    public void on(Color _color){
        ledLight.TurnOn(_color);
    }

    /**
    * @brief Liga o led com a cor especificada.
    *
    * @param color: Cor desejada (padrão vermelho)
    */
    public void on(string _color = "Vermelho")
    {
        on(Color.ToColor(_color));
    }

    /**
    * @brief Liga o led com a cor especificada.
    *
    * @param color: Cor desejada
    */
    public void on(byte red, byte green, byte blue)
    {
        on(new Color(red, green, blue));
    }

    /**
    * @brief Desliga o led.
    *
    */
    public void off()
    {
        ledLight.TurnOff();
    }

    /**
    * @brief Liga ou desliga o led.
    *
    * @param _state: Liga o led se verdadeiro
    */
    public void set(bool _state)
    {
        if (_state)
            on();
        else
            off();
    }

    /**
    * @brief Inverte o estado do led
    *
    */
    public void toggle()
    {
        set(!state);
    }

    /**
    * @brief Pisca o led.
    *
    * @param _time: Tempo de piscada (padrão 0.1s)
    * @param _color: Cor do led (padrão branco)
    */
    public void blink(int _time = 100, string _color = "Branco")
    {
        if(timer.current < lastBlink + _time)
            return;

        toggle();
        lastBlink = timer.current;
    }
}