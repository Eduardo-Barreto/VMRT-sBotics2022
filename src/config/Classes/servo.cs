/**
* @brief Gerencia um motor.
*/


public class motor
{
    /**
	 * @brief Motor a ser gerenciado.
	 *
	 */
    private Servomotor servo;

    /**
	 * @brief Construtor da classe.
	 *
	 * @param motorName: Nome do motor 
	 */
    public motor(string motorName)
    {
        this.servo = Bot.GetComponent<Servomotor>(motorName);
    }

    /**
	 * @brief Indica se o motor está travado.
	 *
	 * @value (bool) Trava o motor se verdadeiro
	 */
    public bool locked
    {
        get => servo.Locked;
        set => servo.Locked = value;
    }

    /**
	 * @brief Indica o ângulo atual do motor (-180 ~ 180).
	 *
	 */
    public double angle
    {
        get => servo.Angle;
    }

    /**
	 * @brief Indica a velocidade atual do motor (-500 ~ 500).
	 *
	 * @value (double) Velocidade desejada
	 */
    public double velocity
    {
        get => servo.Velocity;
        set => servo.Target = value;
    }

    /**
	 * @brief Indica a força atual do motor (0 ~ 500).
	 *
	 * @value (double) Força desejada
	 */
    public double force
    {
        get => servo.Force;
        set => servo.Force = value;
    }

    /**
	 * @brief Move o motor na velocidade e força desejadas.
	 *
	 * @param _velocity: (double) Velocidade desejada (-500 ~ 500).
	 * @param _force: (double) Força desejada (0 ~ 500).
	 */
    public void run(double _velocity, double _force = 500)
    {
        this.velocity = _velocity;
        this.force = _force;
    }
}