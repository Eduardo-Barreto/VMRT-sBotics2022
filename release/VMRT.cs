
public delegate void ActionHandler();


/// <summary>
/// Gerencia o tempo.
/// </summary>

public static class timer{
    /// <summary>
    /// Armazena o tempo inicial do robô em milissegundos
    /// </summary>
    public static long startTime;

    /* /// <summary>
    /// Construtor da classe
    /// </summary>
    public static customTimer(int _startTime = 0){
        startTime = currentUnparsed + _startTime;
    } */

    public static void init(){
        startTime = currentUnparsed;
    }

    /// <summary>
    /// Tempo atual em milissegundos desde 1970
    /// </summary>
    public static long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// Tempo atual em milissegundos desde o início da rotina
    /// </summary>
    public static long current{
        get{
            return currentUnparsed - startTime;
        }
    }

    /// <summary>
    /// Reseta o timer atual
    /// </summary>
    /// <param name="_startTime">(long) Valor alvo para resetar o timer</param>
    public static void resetTimer(long _startTime = 0){
        startTime = current + _startTime;
    }

    /// <summary>
    /// Espera um tempo em milissegundos
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    public static async Task delay(int milliseconds = 50){
        await Time.Delay(milliseconds);
    }

    /// <summary>
    /// Espera um tempo em milissegundos enquanto realiza uma função.
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    /// <param name="doWhileWait">(função) Ação para fazer enquanto espera</param>
    public static async Task delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = current + milliseconds;
        while(current < timeout){
            doWhileWait();
            await delay();
        }
    }
}



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
motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");
motor frontLeftMotor = new motor("frontLeftMotor");
motor frontRightMotor = new motor("frontRightMotor");


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

myRobot robot = new myRobot("leftMotor", "rightMotor", "frontLeftMotor", "frontRightMotor");


/// <summary>
/// Gerencia um sensor de luz
/// </summary>

public class lightSensor
{
    /// <summary>
    /// Sensor de luz a ser gerenciado.
    /// </summary>
    private ColorSensor sensor;

    /// <summary>
    /// Construtor da classe
    /// </summary>
    /// <param name="sensorName">Nome do sensor</param>
    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    /// <summary>
    /// Intensidade da luz refletida pelo sensor (0 ~ 100%).
    /// </summary>
    public double light
    {
        get => sensor.Analog.Brightness/2.55f;
    }

    /// <summary>
    /// Intensidade do vermelho refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double red
    {
        get => sensor.Analog.Red;
    }

    /// <summary>
    /// Intensidade do verde refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double green
    {
        get => sensor.Analog.Green;
    }

    /// <summary>
    /// Intensidade do azul refletido pelo sensor (0 ~ 255).
    /// </summary>
    public double blue
    {
        get => sensor.Analog.Blue;
    }

    /// <summary>
    /// Cor mais próxima identificada pelo sensor.
    /// </summary>
    public string color
    {
        get => sensor.Analog.ToString();
    }

    /// <summary>
    /// Indica se preto é a cor mais próxima identificada pelo sensor.
    /// </summary>
    public bool isColorBlack
    {
        get => !sensor.Digital;
    }
}
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3"),
    new lightSensor("S4")
};

double error, lastError, P, D, PD;
int targetPower = 350;
int turnPower = 400;
long counter = 0;
byte blackTreshold = 50;
byte diffForExit = 15;

byte centerRightLight; // Valor lido do sensor de luz do meio da direita
byte centerLeftLight;  // Valor lido do sensor de luz do meio da esquerda
byte rightLight;       // Valor lido do sensor de luz da direita
byte centerLight;      // Valor lido do sensor de luz do meio
byte leftLight;        // Valor lido do sensor de luz da esquerda
bool rightGreen;       // Indica se existe verde na direita
bool leftGreen;        // Indica se existe verde na esquerda

void readColors(){
    leftLight           = (byte)(lineSensors[0].light);
    centerLeftLight     = (byte)(lineSensors[1].light);
    centerLight         = (byte)(lineSensors[2].light);
    centerRightLight    = (byte)(lineSensors[3].light);
    rightLight          = (byte)(lineSensors[4].light);
}

void runPD()
{
    error = (/*(lineSensors[0].light - lineSensors[4].light) * 1.2 + */(lineSensors[1].light - lineSensors[3].light))/* / 2.2*/;

    P = error * 45;
    D = (error - lastError) * 0;
    lastError = error;

    PD = P + D;

    robot.move(targetPower + PD, targetPower - PD);
    IO.PrintLine(Convert.ToInt32(lineSensors[1].light).ToString() + "\t|\t" + Convert.ToInt32(lineSensors[3].light).ToString());
    counter++;

}

 async Task runLineFollower()
{
    readColors();
    IO.PrintLine(centerLeftLight.ToString() + "\t" + centerRightLight.ToString());

    if(centerLeftLight < blackTreshold){
        robot.move(0, 0);
        long timeout = timer.current + 350;

        while(timer.current < timeout)
        {
            readColors();
            if(centerLeftLight > blackTreshold+diffForExit || centerRightLight < blackTreshold || centerLight < blackTreshold)
                break;
            robot.turn(-turnPower);
            await timer.delay();
        }
        robot.move(0, 0);

    }

    else if(centerRightLight < blackTreshold){
        robot.move(0, 0);
        long timeout = timer.current + 350;

        while(timer.current < timeout)
        {
            readColors();
            if(centerRightLight > blackTreshold+diffForExit || centerLeftLight < blackTreshold || centerLight < blackTreshold)
                break;
            robot.turn(turnPower);
            await timer.delay();
        }
        robot.move(0, 0);

    }

    robot.moveStraight(targetPower);
}

async Task setup()
{
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    timer.init();
    await timer.delay(300);
    robot.locked = false;
    await robot.moveStraightTime(100, 300);
}

async Task loop()
{
    await runLineFollower();
}

async Task Main()
{
    await setup();
    for (; ; )
    {
        await loop();
        await timer.delay();
    }
}
