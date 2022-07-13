/// <summary>
/// Gerencia a base do robô
/// </summary>

public class robotBase
{
    /// <summary>
    /// Motores da base do robô.
    /// </summary>
    private motor leftMotor, rightMotor;

    /// <summary>
    /// Construtor da classe.
    /// </summary>
    /// <param name="leftMotorName">(String) Nome do motor esquerdo.</param>
    /// <param name="rightMotorName">(String) Nome do motor direito.</param>
    public robotBase(string leftMotorName, string rightMotorName)
    {
        this.leftMotor = new motor(leftMotorName);
        this.rightMotor = new motor(rightMotorName);
    }

    /// <summary>
    /// Propriedade que indica se os motores do robô estão travados.
    /// </summary>
    public bool locked
    {
        get => (leftMotor.locked || rightMotor.locked);
        set
        {
            leftMotor.locked = value;
            rightMotor.locked = value;
        }
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
        this.locked = !forceUnlock;
        leftMotor.run(leftVelocity, leftForce);
        rightMotor.run(rightVelocity, rightForce);
    }

    /// <summary>
    /// Método que move a base do robô em curva com a velocidade e força especificada.
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô na curva.</param>
    /// <param name="force">(double) Força do robô na curva.</param>
    public void turn(double velocity, double force = 500)
    {
        this.move(velocity, -velocity, force, force);
    }

    /// <summary>
    /// Método que move a base do robô em linha reta com a velocidade e força especificada.
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em linha reta.</param>
    /// <param name="force">(double) Força do robô em linha reta.</param>
    public void moveStraight(double velocity, double force = 500)
    {
        this.move(velocity, velocity, force, force);
    }

    /// <summary>
    /// Método que para o robô.
    /// </summary>
    /// <param name="time">(int) Tempo para ficar parado.</param>
    /// <param name="lock">(bool) Indica se deve travar os motores após o movimento.</param>
    public void stop(int time = 50, bool _lock = true)
    {
        this.move(0, 0);
        this.locked = _lock;
    }

    /// <summary>
    /// Método que move o robô em linha reta durante um determinado tempo
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em linha reta.</param>
    /// <param name="force">(double) Força do robô em linha reta.</param>
    /// <param name="time">(int) Tempo para o robô ficar em linha reta.</param>
    public void moveStraightTime(double velocity, double force = 500, int time = 50)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            this.moveStraight(velocity, force);
            timer.delay();
        }
        this.stop();
    }

    /// <summary>
    /// Método que move o robô em curva durante um determinado tempo
    /// </summary>
    /// <param name="velocity">(double) Velocidade do robô em curva.</param>
    /// <param name="force">(double) Força do robô em curva.</param>
    /// <param name="time">(int) Tempo para o robô ficar em curva.</param>
    public void turnTime(double velocity, double force = 500, int time = 50)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            this.turn(velocity, force);
            timer.delay();
        }
        this.stop();
    }

}