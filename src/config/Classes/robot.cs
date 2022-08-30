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

    public double inclination
    {
        get => Bot.Inclination;
    }

    public double compass
    {
        get => Bot.Compass;
    }

    public int brickSpeed
    {
        get => (int)(Bot.Speed);
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
    public int leftVelocity{
        get => leftMotor.velocity;
        set => leftMotor.velocity = value;
    }

    /**
	 * @brief Propriedade que indica a velocidade do motor da direita.
	 *
	 */
    public int rightVelocity{
        get => rightMotor.velocity;
        set => rightMotor.velocity = value;
    }

    /**
	 * @brief Método que move a base do robô com a velocidade e força especificada.
	 *
	 * @param leftVelocity: (int) Velocidade do motor esquerdo.
	 * @param rightVelocity: (int) Velocidade do motor direito.
	 * @param leftForce: (byte) Força do motor esquerdo.
	 * @param rightForce: (byte) Força do motor direito.
	 * @param forceUnlock: (bool) Força o destravamento dos motores se verdadeiro
	 */
    public void move(int leftVelocity, int rightVelocity, byte leftForce = 10, byte rightForce = 10, bool forceUnlock = true)
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
	 * @param velocity: (int) Velocidade do robô na curva.
	 * @param force: (byte) Força do robô na curva.
	 */
    public void turn(int velocity, byte force = 10)
    {
        move(velocity, -velocity, force, force);
    }

    /**
	 * @brief Método que move a base do robô em linha reta com a velocidade e força especificada.
	 *
	 * @param velocity: (int) Velocidade do robô em linha reta.
	 * @param force: (byte) Força do robô em linha reta.
	 */
    public void moveStraight(int velocity, byte force = 10)
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
        move(-leftVelocity, -rightVelocity, 100, 100);
        await timer.delay();
        move(0, 0, 100, 100);
        locked = _lock;
        await timer.delay(time);
        locked = !_lock;
    }


    /**
	 * @brief Método que move o robô durante um determinado tempo
	 *
	 * @param leftVelocity: (int) Velocidade do motor esquerdo.
	 * @param rightVelocity: (int) Velocidade do motor direito.
     * @param time: (int) Tempo para andar.
     * @param leftForce: (byte) Força do motor esquerdo.
     * @param rightForce: (byte) Força do motor direito.
	 */
    public async Task moveTime(int leftVelocity, int rightVelocity, int time = 50, byte leftForce = 10, byte rightForce = 10){
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            move(leftVelocity, rightVelocity, leftForce, rightForce);
            await timer.delay();
        }
        await stop();
    }

    /**
	 * @brief Método que move o robô em linha reta durante um determinado tempo
	 *
	 * @param velocity: (int) Velocidade do robô em linha reta.
	 * @param force: (byte) Força do robô em linha reta.
	 * @param time: (int) Tempo para o robô ficar em linha reta.
	 */
    public async Task moveStraightTime(int velocity, int time = 50, byte force = 10, bool stopAfter = true)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            moveStraight(velocity, force);
            await timer.delay();
        }
        if(stopAfter)
            await stop();
    }

    /**
	 * @brief Método que move o robô em curva durante um determinado tempo
	 *
	 * @param velocity: (int) Velocidade do robô em curva.
	 * @param force: (byte) Força do robô em curva.
	 * @param time: (int) Tempo para o robô ficar em curva.
	 */
    public async Task turnTime(int velocity, int time = 50, byte force = 10, bool stopAfter = true)
    {
        long timeout = timer.current + time;
        while (timer.current < timeout)
        {
            turn(velocity, force);
            await timer.delay();
        }
        if(stopAfter)
            await stop();
    }

    public async Task turnDegrees(int degrees, int velocity, byte force = 10, bool fast = false){
        int turnSide = (degrees > 0) ? 1 : -1;
        int targetAngle = convertDegrees((compass) + degrees);

        while(!proximity(compass, targetAngle, 20)){
            turn(velocity * turnSide, force);
            await timer.delay();
        }

        if(fast){
            await stop();
            return;
        }

        while(!proximity(compass, targetAngle, 1)){
            turn(5 * turnSide, force);
            await timer.delay();
        }

        await stop();
    }

    public async Task moveToAngle(int targetAngle, int velocity, byte force = 10){
        int turnSide = (targetAngle > compass) ? 1 : -1;
        turnSide = (Math.Abs(targetAngle - compass) > 180) ? -turnSide : turnSide;

        while(!proximity(compass, targetAngle, 20)){
            turn(velocity * turnSide, force);
            await timer.delay();
        }

        while(!proximity(compass, targetAngle, 1)){
            turn(5 * turnSide, force);
            await timer.delay();
        }

        await stop();
    }

    public async Task alignAngle(){

        await stop();
        double angle = compass;

        if ((angle > 315) || (angle <= 45))
        {
            await moveToAngle(0, 30);
        }
        else if ((angle > 45) && (angle <= 135))
        {
            await moveToAngle(90, 30);
        }
        else if ((angle > 135) && (angle <= 225))
        {
            await moveToAngle(180, 30);
        }
        else if ((angle > 225) && (angle <= 315))
        {
            await moveToAngle(270, 30);
        }
        await stop();
    }

    public async Task die(){
        await stop(100);
        locked = true;
        await stop(int.MaxValue);
        while(true){
            await timer.delay();
        }
    }

}