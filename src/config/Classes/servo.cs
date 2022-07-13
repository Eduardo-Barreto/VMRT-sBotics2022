/// <summary>
/// Gerencia um motor.
/// </summary>

public class motor
{
    /// <summary>
    /// Motor a ser gerenciado.
    /// </summary>
    private Servomotor servo;

    /// <summary>
    /// Construtor da classe.
    /// </summary>
    /// <param name="motorName">Nome do motor</param>
    public motor(string motorName)
    {
        this.servo = Bot.GetComponent<Servomotor>(motorName);
    }

    /// <summary>
    /// Indica se o motor está travado.
    /// </summary>
    /// <value>(bool) Trava o motor se verdadeiro</value>
    public bool locked
    {
        get => servo.Locked;
        set => servo.Locked = value;
    }

    /// <summary>
    /// Indica o ângulo atual do motor (-180 ~ 180).
    /// </summary>
    /// <value></value>
    public double angle
    {
        get => servo.Angle;
    }

    /// <summary>
    /// Indica a velocidade atual do motor (-500 ~ 500).
    /// </summary>
    /// <value>(double) Velocidade desejada</value>
    public double velocity
    {
        get => servo.Velocity;
        set => servo.Target = value;
    }

    /// <summary>
    /// Indica a força atual do motor (0 ~ 500).
    /// </summary>
    /// <value>(double) Força desejada</value>
    public double force
    {
        get => servo.Force;
        set => servo.Force = value;
    }

    /// <summary>
    /// Move o motor na velocidade e força desejadas.
    /// </summary>
    /// <param name="_velocity">(double) Velocidade desejada (-500 ~ 500).</param>
    /// <param name="_force">(double) Força desejada (0 ~ 500).</param>
    public void run(double _velocity, double _force = 500)
    {
        this.velocity = _velocity;
        this.force = _force;
    }
}