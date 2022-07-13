
public delegate void ActionHandler();


/// <summary>
/// Gerencia o tempo.
/// </summary>

public class myTimer{
    /// <summary>
    /// Armazena o tempo inicial do robô em milissegundos
    /// </summary>
    public long startTime;

    /// <summary>
    /// Construtor da classe
    /// </summary>
    public myTimer(){
        this.startTime = this.currentUnparsed;
    }

    /// <summary>
    /// Tempo atual em milissegundos desde 1970
    /// </summary>
    public long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// Tempo atual em milissegundos desde o início da rotina
    /// </summary>
    public long current{
        get{
            return this.currentUnparsed - this.startTime;
        }
    }

    /// <summary>
    /// Reseta o timer atual
    /// </summary>
    /// <param name="_startTime">(long) Valor alvo para resetar o timer</param>
    public void resetTimer(long _startTime = 0){
        this.startTime = this.current + _startTime;
    }

    /// <summary>
    /// Espera um tempo em milissegundos
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    public async Task delay(int milliseconds = 50){
        await Time.Delay(milliseconds);
    }

    /// <summary>
    /// Espera um tempo em milissegundos enquanto realiza uma função.
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    /// <param name="doWhileWait">(função) Ação para fazer enquanto espera</param>
    public void delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = this.current + (milliseconds/50);
        while(current < timeout){
            doWhileWait();
            this.delay();
        }
    }
}
myTimer timer = new myTimer();


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
robotBase robot = new robotBase("leftMotor", "rightMotor");


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
int targetPower = 450;
int turnPower = 500;
long counter = 0;
byte blackTreshold = 100;

byte centerRightLight; // Valor lido do sensor de luz do meio da direita
byte centerLeftLight;  // Valor lido do sensor de luz do meio da esquerda
byte rightLight;       // Valor lido do sensor de luz da direita
byte leftLight;        // Valor lido do sensor de luz da esquerda
byte borderRightLight; // Valor lido do sensor de luz da borda da direita
byte borderLeftLight;  // Valor lido do sensor de luz da borda da esquerda
bool rightGreen;       // Indica se existe verde na direita
bool leftGreen;        // Indica se existe verde na esquerda

void readColors(){
    leftLight           = (byte)(lineSensors[0].light);
    centerLeftLight     = (byte)(lineSensors[1].light);
    centerRightLight    = (byte)(lineSensors[3].light);
    rightLight          = (byte)(lineSensors[4].light);
}

void runPD()
{
    error = ((lineSensors[0].light - lineSensors[4].light) * 1.2 + (lineSensors[1].light - lineSensors[3].light)) / 2.2;

    P = error * 45;
    D = (error - lastError) * -5;
    lastError = error;

    PD = P + D;

    robot.move(targetPower - PD, targetPower + PD);
    IO.PrintLine(counter.ToString() + "," + PD.ToString());
    counter++;

}

void runLineFollower()
{
    readColors();
    IO.PrintLine(centerLeftLight.ToString() + "\t" + centerRightLight.ToString());

    if(leftLight < blackTreshold){
        turnPower = -500;
    }
    if(rightLight < blackTreshold){
        turnPower = 500;
    }
    else{
        robot.moveStraight(targetPower);
        return;
    }

    robot.move(turnPower, -turnPower);
}

void setup()
{
    robot.locked = false;
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
}


void loop()
{
    runLineFollower();
}

async Task Main()
{
    setup();
    for (; ; )
    {
        loop();
        await timer.delay();
    }
}

