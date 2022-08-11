/// <summary>
/// Gerencia a base do robô
/// </summary>

public class myRobot
{
    /// <summary>
    /// Motores da base do robô.
    /// </summary>
    private motor leftMotor;
    private motor rightMotor;
    private motor frontLeftMotor;
    private motor frontRightMotor;

    /// <summary>
    /// Construtor da classe.
    /// </summary>
    /// <param name="leftMotorName">(String) Nome do motor esquerdo.</param>
    /// <param name="rightMotorName">(String) Nome do motor direito.</param>
    public myRobot(string leftMotorName, string rightMotorName, string frontLeftMotorName, string frontRightMotorName)
    {
        this.leftMotor = new motor(leftMotorName);
        this.rightMotor = new motor(rightMotorName);
        this.frontLeftMotor = new motor(frontLeftMotorName);
        this.frontRightMotor = new motor(frontRightMotorName);
    }

    /// <summary>
    /// Propriedade que indica se os motores do robô estão travados.
    /// </summary>
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

    public double leftVelocity{
        get => leftMotor.velocity;
        set => leftMotor.velocity = value;
    }

    public double rightVelocity{
        get => rightMotor.velocity;
        set => rightMotor.velocity = value;
    }

    /// <summary>
    /// Método que move a base do robô com a velocidade e força especificada.
    /// </summary>
    /// <param name="leftVelocity">(double) Velocidade do motor esquerdo.</param>
    /// <param name="leftForce">(double) Força do motor esquerdo.</param>
    /// <param name="rightVelocity">(double) Velocidade do motor direito.</param>
    /// <param name="rightForce">(double) Força do motor direito.</param>
    /// <param name="forceUnlock">(bool) Força o destravamento dos motores se verdadeiro</param>
    public void move(double leftVelocity, double rightVelocity, double leftForce = 500, double rightForce = 500, bool forceUnlock = true)
    {
        locked = !forceUnlock;
        leftMotor.run(leftVelocity, leftForce);
        frontLeftMotor.run(leftVelocity, leftForce);
        rightMotor.run(rightVelocity, rightForce);
        frontRightMotor.run(rightVelocity, rightForce);
    }

    /// <summary>
    /// Método que move a base do robô em curva com a velocidade e força especificada.
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô na curva.</param>
    /// <param name="force">(double) Força do robô na curva.</param>
    public void turn(double velocity, double force = 500)
    {
        move(velocity, -velocity, force, force);
    }

    /// <summary>
    /// Método que move a base do robô em linha reta com a velocidade e força especificada.
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em linha reta.</param>
    /// <param name="force">(double) Força do robô em linha reta.</param>
    public void moveStraight(double velocity, double force = 500)
    {
        move(velocity, velocity, force, force);
    }

    /// <summary>
    /// Método que para o robô.
    /// </summary>
    /// <param name="time">(int) Tempo para ficar parado.</param>
    /// <param name="lock">(bool) Indica se deve travar os motores após o movimento.</param>
    public async Task stop(int time = 50, bool _lock = true)
    {
        move(-leftVelocity, -rightVelocity);
        await timer.delay();
        move(0, 0);
        locked = _lock;
        await timer.delay(time);
        locked = !_lock;
    }

    public async Task moveTime(double leftVelocity, double rightVelocity, int time = 50, double leftForce = 500, double rightForce = 500){
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            move(leftVelocity, rightVelocity, leftForce, rightForce);
            await timer.delay();
        }
        stop();
    }

    /// <summary>
    /// Método que move o robô em linha reta durante um determinado tempo
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em linha reta.</param>
    /// <param name="force">(double) Força do robô em linha reta.</param>
    /// <param name="time">(int) Tempo para o robô ficar em linha reta.</param>
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

    /// <summary>
    /// Método que move o robô em curva durante um determinado tempo
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em curva.</param>
    /// <param name="force">(double) Força do robô em curva.</param>
    /// <param name="time">(int) Tempo para o robô ficar em curva.</param>
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