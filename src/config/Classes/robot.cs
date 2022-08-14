/**
 * @brief Gerencia a base do robô
 *
*/
public class myRobot
{
    /**
	 * @brief Motores da base do robô.
	 *
	 */
    private motor leftMotor;
    private motor rightMotor;
    private motor frontLeftMotor;
    private motor frontRightMotor;

    /**
	 * @brief Construtor da classe.
	 *
	 * @param leftMotorName: (String) Nome do motor esquerdo.
	 * @param rightMotorName: (String) Nome do motor direito.
     * @param frontLeftMotorName: (String) Nome do motor frontal esquerdo do robo.
     * @param frontRightMotorName: (String) Nome do motor frontal direito do robo.
	 */
    public myRobot(string leftMotorName, string rightMotorName, string frontLeftMotorName, string frontRightMotorName)
    {
        this.leftMotor = new motor(leftMotorName);
        this.rightMotor = new motor(rightMotorName);
        this.frontLeftMotor = new motor(frontLeftMotorName);
        this.frontRightMotor = new motor(frontRightMotorName);
    }

    /**
	 * @brief Propriedade que indica se os motores do robô estão travados.
	 *
	 */
    public bool locked
    {
        get => (leftMotor.locked || rightMotor.locked || frontLeftMotor.locked || frontRightMotor.locked);
        set
        {
            leftMotor.locked = value;
            rightMotor.locked = value;
            frontLeftMotor.locked = value;
            frontRightMotor.locked = value;
        }
    }

    /**
	 * @brief Propriedade que indica a velocidade do motor da esquerda.
	 *
	 */
    public double leftVelocity{
        get => leftMotor.velocity;
        set => leftMotor.velocity = value;
    }

    /**
	 * @brief Propriedade que indica a velocidade do motor da direita.
	 *
	 */
    public double rightVelocity{
        get => rightMotor.velocity;
        set => rightMotor.velocity = value;
    }

    /**
	 * @brief Método que move a base do robô com a velocidade e força especificada.
	 *
	 * @param leftVelocity: (double) Velocidade do motor esquerdo.
	 * @param leftForce: (double) Força do motor esquerdo.
	 * @param rightVelocity: (double) Velocidade do motor direito.
	 * @param rightForce: (double) Força do motor direito.
	 * @param forceUnlock: (bool) Força o destravamento dos motores se verdadeiro
	 */
    public void move(double leftVelocity, double rightVelocity, double leftForce = 500, double rightForce = 500, bool forceUnlock = true)
    {
        locked = !forceUnlock;
        leftMotor.run(leftVelocity, leftForce);
        frontLeftMotor.run(leftVelocity, leftForce);
        rightMotor.run(rightVelocity, rightForce);
        frontRightMotor.run(rightVelocity, rightForce);
    }

    /**
	 * @brief Método que move a base do robô em curva com a velocidade e força especificada.
	 *
	 * @param velocity: (double) Velocidade do robô na curva.
	 * @param force: (double) Força do robô na curva.
	 */
    public void turn(double velocity, double force = 500)
    {
        move(velocity, -velocity, force, force);
    }

    /**
	 * @brief Método que move a base do robô em linha reta com a velocidade e força especificada.
	 *
	 * @param velocity: (double) Velocidade do robô em linha reta.
	 * @param force: (double) Força do robô em linha reta.
	 */
    public void moveStraight(double velocity, double force = 500)
    {
        move(velocity, velocity, force, force);
    }

    /**
	 * @brief Método que para o robô.
	 *
	 * @param time: (int) Tempo para ficar parado.
	 * @param lock: (bool) Indica se deve travar os motores após o movimento.
	 */
    public async Task stop(int time = 50, bool _lock = true)
    {
        move(-leftVelocity, -rightVelocity);
        await timer.delay();
        move(0, 0);
        locked = _lock;
        await timer.delay(time);
        locked = !_lock;
    }


    /**
	 * @brief Método que move o robô durante um determinado tempo
	 *
	 * @param leftVelocity: (double) Velocidade do motor esquerdo.
	 * @param rightVelocity: (double) Velocidade do motor direito.
     * @param time: (int) Tempo para andar.
     * @param leftForce: (double) Força do motor esquerdo.
     * @param rightForce: (double) Força do motor direito.
	 */
    public async Task moveTime(double leftVelocity, double rightVelocity, int time = 50, double leftForce = 500, double rightForce = 500){
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            move(leftVelocity, rightVelocity, leftForce, rightForce);
            await timer.delay();
        }
        stop();
    }

    /**
	 * @brief Método que move o robô em linha reta durante um determinado tempo
	 *
	 * @param velocity: (double) Velocidade do robô em linha reta.
	 * @param force: (double) Força do robô em linha reta.
	 * @param time: (int) Tempo para o robô ficar em linha reta.
	 */
    public async Task moveStraightTime(double velocity, int time = 50, bool stopAfter = true, double force = 500)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            moveStraight(velocity, force);
            await timer.delay();
        }
        if(stopAfter)
            stop();
    }

    /**
	 * @brief Método que move o robô em curva durante um determinado tempo
	 *
	 * @param velocity: (double) Velocidade do robô em curva.
	 * @param force: (double) Força do robô em curva.
	 * @param time: (int) Tempo para o robô ficar em curva.
	 */
    public async Task turnTime(double velocity, int time = 50, bool stopAfter = true, double force = 500)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            turn(velocity, force);
            await timer.delay();
        }
        if(stopAfter)
            stop();
    }

}