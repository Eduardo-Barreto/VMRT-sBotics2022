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
	 * @brief Indica a velocidade atual do motor (-100 ~ 100).
	 *
	 * @value (double) Velocidade desejada
	 */
    public int velocity
    {
        get => (int)(servo.Velocity/5);
        set => servo.Target = (double)(value*5);
    }

    /**
	 * @brief Indica a força atual do motor (0 ~ 500).
	 *
	 * @value (double) Força desejada
	 */
    public int force
    {
        get => (byte)(servo.Force/5);
        set => servo.Force = (double)(value*5);
    }

    /**
	 * @brief Move o motor na velocidade e força desejadas.
	 *
	 * @param _velocity: (int) Velocidade desejada (-100 ~ 100).
	 * @param _force: (byte) Força desejada (0 ~ 500).
	 */
    public void run(int _velocity, byte _force = 100)
    {
        this.velocity = _velocity*5;
        this.force = _force*5;
    }
}