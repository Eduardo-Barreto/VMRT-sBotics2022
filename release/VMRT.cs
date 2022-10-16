
static double map(double val, double inMin, double inMax, double outMin, double outMax)
{
    // "mapeia" ou reescala um val (val), de uma escala (inMin~inMax) para outra (outMin~outMax)
    return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
}

static bool interval(double val, double min, double max)
{
    // verifica se um valor (val) está dentro de um intervalo (min~max)
    return val >= min && val <= max;
}

static bool proximity(double val, double target, double tolerance = 1)
{
    // verifica se um valor (val) está proximo de um alvo (target) com tolerância (tolerance)
    return interval(val, target - tolerance, target + tolerance);
}

static int convertDegrees(double degrees){
    // converte um angulo em graus para sempre se manter entre 0~360
    return (int)((degrees % 360 + 360) % 360);
}

static bool degreesProximity(double degrees, double target, double tolerance = 1){
    // verifica se um angulo (degrees) está proximo de um alvo (target) com tolerância (tolerance)
    return interval(
        convertDegrees(degrees),
        convertDegrees(target - tolerance),
        convertDegrees(target + tolerance)
    );
}

static double constrain(double val, double min, double max)
{
    // limita um valor (val) entre um intervalo (min~max)
    return val < min ? min : val > max ? max : val;
}

public delegate void ActionHandler();


/**
 * @brief Gerencia o tempo.
 *
 */

public static class timer{
    /**
	 * @brief Armazena o tempo inicial do robô em milissegundos
	 *
	 */
    public static long startTime;

    public static void init(){
        startTime = currentUnparsed;
    }

    /**
	 * @brief Tempo atual em milissegundos desde 1970
	 */
    public static long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /**
	 * @brief Tempo atual em milissegundos desde o início da rotina
	 */
    public static long current{
        get{
            return currentUnparsed - startTime;
        }
    }

    /**
	 * @brief Reseta o timer atual
	 *
	 * @param _startTime: (long) Valor alvo para resetar o timer
	 */
    public static void resetTimer(long _startTime = 0){
        startTime = current + _startTime;
    }

    /**
	 * @brief Espera um tempo em milissegundos
	 *
	 * @param milliseconds: (int) Tempo a esperar
	 */
    public static async Task delay(int milliseconds = 1){
        await Time.Delay(milliseconds);
    }

    /**
	 * @brief Espera um tempo em milissegundos enquanto realiza uma função.
	 *
	 * @param milliseconds: (int) Tempo a esperar
	 * @param doWhileWait: (função) Ação para fazer enquanto espera
	 */
    public static async Task delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = current + milliseconds;
        while(current < timeout){
            doWhileWait();
            await delay();
        }
    }
}



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

    public async Task stop(int time = 50, bool _lock = true)
    {
        run(-velocity, 100);
        await timer.delay();
        run(0, 100);
        locked = _lock;
        await timer.delay(time);
        locked = !_lock;
    }

    public async Task moveTime(int velocity, int time = 50, byte force = 10){
        run(velocity, force);
        await timer.delay(time);
        await stop();
    }
}
motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");
motor frontLeftMotor = new motor("frontLeftMotor");
motor frontRightMotor = new motor("frontRightMotor");



/**
* Gerencia um sensor ultrassônico
*/

public class ultrasonic{
    private UltrasonicSensor sensor; // Sensor ultrassônico a ser gerenciado
    private long lastReadTime = 0;
    private float lastRead = -1;

    /**
    * @brief Construtor da classe
    *
    *
    * @param sensorName: (string) nome do sensor
    */
    public ultrasonic(string sensorName){
        this.sensor = Bot.GetComponent<UltrasonicSensor>(sensorName);
    }

    public float read{
        get{
            if(timer.current < lastReadTime + 100)
                return lastRead;

            lastRead = (float)(sensor.Analog);
            lastReadTime = timer.current;

            return lastRead;
        }
    }
}
ultrasonic[] frontUltra ={
    new ultrasonic("centerUltra0"),
    new ultrasonic("centerUltra1")
};

ultrasonic leftUltra = new ultrasonic("leftUltra");



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
    private ultrasonic ultra;

    /**
	 * @brief Construtor da classe.
	 *
	 * @param leftMotorName: (String) Nome do motor esquerdo.
	 * @param rightMotorName: (String) Nome do motor direito.
     * @param frontLeftMotorName: (String) Nome do motor frontal esquerdo do robo.
     * @param frontRightMotorName: (String) Nome do motor frontal direito do robo.
	 */
    public myRobot(string leftMotorName, string rightMotorName, string frontLeftMotorName, string frontRightMotorName, string ultrasonicName)
    {
        this.leftMotor = new motor(leftMotorName);
        this.rightMotor = new motor(rightMotorName);
        this.frontLeftMotor = new motor(frontLeftMotorName);
        this.frontRightMotor = new motor(frontRightMotorName);
        this.ultra = new ultrasonic(ultrasonicName);
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
        int targetAngle = convertDegrees(compass + degrees);

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

        if(targetAngle != 0 || compass < 315){
            while(!proximity(compass, targetAngle, 20)){
                turn(velocity * turnSide, force);
                await timer.delay();
            }
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

    public async Task alignUltra(int distance, int velocity, byte times = 3, byte force = 10){
        for(byte i = 1; i <= times; i++){
            int distanceSide = (distance - ultra.read > 0) ? 1 : -1;
            IO.PrintLine(i.ToString() + " - " + velocity.ToString() + " - " + (ultra.read).ToString());

            while(!proximity(ultra.read, distance, 0.2f)){
                velocity = (velocity/i) * distanceSide;
                move(velocity, velocity, force, force);
                IO.PrintLine(i.ToString() + " - " + velocity.ToString() + " - " + (ultra.read).ToString());
                await timer.delay();
            }
            await stop(150);
        }
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
myRobot robot = new myRobot("leftMotor", "rightMotor", "frontLeftMotor", "frontRightMotor", "centerUltra0");


/**
* Gerencia um sensor de luz
*/

public class lightSensor
{
    private ColorSensor sensor; // Sensor de luz a ser gerenciado
    private byte grayRed;       // Valor de vermelho para o sensor estar em cinza
    private byte grayGreen;     // Valor de verde para o sensor estar em cinza
    private byte grayBlue;      // Valor de azul para o sensor estar em cinza

    /**
     * @brief Construtor da classe
     *
     *
     * @param sensorName: (string) Sensor de luz a ser gerenciado
     */
    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    /**
     * @brief Retorna o valor do sensor de luz
     *
     *
     * @return (double) Valor do sensor de luz (0~100%)
     */
    public double light
    {
        get => sensor.Analog.Brightness/2.55f;
    }

    /**
     * @brief Retorna a intensidade do vermelho refletido pelo sensor
     *
     *
     * @return (double) Intensidade do vermelho refletido pelo sensor (0~255)
     */
    public double red
    {
        get => sensor.Analog.Red;
    }

    /**
     * @brief Retorna a intensidade do verde refletido pelo sensor
     *
     *
     * @return (double) Intensidade do verde refletido pelo sensor (0~255)
     */
    public double green
    {
        get => sensor.Analog.Green;
    }

    /**
     * @brief Retorna a intensidade do azul refletido pelo sensor
     *
     *
     * @return (double) Intensidade do azul refletido pelo sensor (0~255)
     */
    public double blue
    {
        get => sensor.Analog.Blue;
    }

    /**
    * @brief Retorna a cor mais próxima identificada pelo sensor
    */
    public string color
    {
        get => sensor.Analog.ToString();
    }

    /**
    * @brief Indica se preto é a cor mais próxima identificada pelo sensor
    */
    public bool isColorBlack
    {
        get => !sensor.Digital;
    }

    public bool isGreen
    {
        get => green > red + 15 && green > blue + 15;
    }

    public void setGray(byte red, byte green, byte blue)
    {
        this.grayRed = red;
        this.grayGreen = green;
        this.grayBlue = blue;
    }

    public byte isGray
    {
        get => (proximity(grayRed, red) && proximity(grayGreen, green) && proximity(grayBlue, blue)) || color == "Azul" ? (byte)(1) : (byte)(0);
    }

    public byte isRed{
        get => red > green + 15 && red > blue + 15 ? (byte)(1) : (byte)(0);
    }

    /* metodo antigo
    bool verde(byte sensor)
    {
        float val_vermelho = bot.ReturnRed(sensor);
        float val_verde = bot.ReturnGreen(sensor);
        float val_azul = bot.ReturnBlue(sensor);
        byte media_vermelho = 13, media_verde = 82, media_azul = 4;
        int RGB = (int)(val_vermelho + val_verde + val_azul);
        sbyte vermelho = (sbyte)(map(val_vermelho, 0, RGB, 0, 100));
        sbyte verde = (sbyte)(map(val_verde, 0, RGB, 0, 100));
        sbyte azul = (sbyte)(map(val_azul, 0, RGB, 0, 100));
        return ((proximo(vermelho, media_vermelho, 2) && proximo(verde, media_verde, 2) && proximo(azul, media_azul, 2)) || cor(sensor) == "VERDE");
    }
    */
}
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3")
};


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
led[][] leds ={
    new led[] {
        new led("L01"),
        new led("L01"),
        new led("L02"),
        new led("L02")
    },
    new led[] {
        new led("L10"),
        new led("L11"),
        new led("L12"),
        new led("L13")
    },
    new led[] {
        new led("L20"),
        new led("L21"),
        new led("L22"),
        new led("L23")
    }
};

void lineLeds(string color){
    for(byte i = 0; i<= 3; i++)
        leds[1][i].on(color);
}

void arrowLeds(string color, byte side){
    lineLeds(color);

    for(byte i = 0; i<= 2; i++)
        leds[i][side].on(color);
}

void turnOnAllLeds(string color){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.on(color);
        }
    }
}

void turnOnAllLeds(Color color){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.on(color);
        }
    }
}

void turnOnAllLeds(byte red, byte green, byte blue){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.on(red, green, blue);
        }
    }
}

void turnOffAllLeds(){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.off();
        }
    }
}

int targetPower = 10;
int turnPower = 10;
byte blackTreshold = 15;
byte blackTresholdTurn = 25;
byte diffForExit = 15;

byte centerRightLight; // Valor lido do sensor de luz do meio da direita
byte centerLeftLight;  // Valor lido do sensor de luz do meio da esquerda
byte rightLight;       // Valor lido do sensor de luz da direita
byte leftLight;        // Valor lido do sensor de luz da esquerda

bool centerRightBlack; // Se o sensor de luz do meio da direita está preto
bool centerLeftBlack;  // Se o sensor de luz do meio da esquerda está preto
bool rightBlack;       // Se o sensor de luz da direita está preto
bool leftBlack;        // Se o sensor de luz da esquerda está preto

bool rightGreen;       // Indica se existe verde na direita
bool leftGreen;        // Indica se existe verde na esquerda

bool gray;            // Indica se existe uma linha cinza
bool red;             // Indica se existe uma linha vermelha

void setGray(byte red, byte green, byte blue)
{
    foreach(lightSensor sensor in lineSensors)
    {
        sensor.setGray(red, green, blue);
    }
}

void readColors(int offset = 0){
    leftLight           = (byte)(lineSensors[0].light);
    centerLeftLight     = (byte)(lineSensors[1].light);
    centerRightLight    = (byte)(lineSensors[2].light);
    rightLight          = (byte)(lineSensors[3].light);

    leftBlack           = (leftLight < blackTresholdTurn + offset);
    centerLeftBlack     = (centerLeftLight < blackTreshold + offset);
    centerRightBlack    = (centerRightLight < blackTreshold + offset);
    rightBlack          = (rightLight < blackTresholdTurn + offset);

    leftGreen           = (lineSensors[0].isGreen || lineSensors[1].isGreen);
    rightGreen          = (lineSensors[2].isGreen || lineSensors[3].isGreen);

    gray = !afterRescue && ((lineSensors[0].isGray + lineSensors[1].isGray + lineSensors[2].isGray + lineSensors[3].isGray) >= 2);
    red = afterRescue && ((lineSensors[0].isRed + lineSensors[1].isRed + lineSensors[2].isRed + lineSensors[3].isRed) >= 2);

}

async Task alignLine(){
    while(leftBlack || centerLeftBlack){
        readColors();
        robot.turn(-10);
        await timer.delay();
    }
    await robot.stop();
    while(rightBlack || centerRightBlack){
        readColors();
        robot.turn(10);
        await timer.delay();
    }
    await robot.stop();
}


long lastTurnTime = 0;

async Task returnRoutine(){
    await robot.stop();
    readColors();
    await alignLine();

    turnOffAllLeds();
    lastTurnTime = timer.current;
    return;
}

async Task<bool> checkDeadEnd(){
    if(leftGreen && rightGreen){
        arrowLeds("Verde", 1);
        arrowLeds("Verde", 2);
        await robot.stop(150);
        await robot.moveStraightTime(15, 700, 1);
        await robot.stop(500);

        await robot.turnDegrees(170, 30, 10, true);

        readColors();
        while(!centerLeftBlack && !centerRightBlack){
            readColors();
            robot.turn(10);
            await timer.delay();
        }
        await robot.stop();

        await returnRoutine();
        return true;
    }
    return false;
}

async Task <bool> checkGreen(){
    if(timer.current - lastTurnTime < 750)
        return false;

    if(gray)
        return true;

    int turnForce = 0;
    if(leftGreen){
        turnForce = -10;
        arrowLeds("Verde", 1);
    }
    else if(rightGreen){
        turnForce = 10;
        arrowLeds("Verde", 2);
    }
    else
        return false;

    await robot.stop(150);

    if(await checkDeadEnd())
        return true;

    readColors();

    if(await checkDeadEnd())
        return true;

    await robot.moveStraightTime(15, 700, 1);
    await robot.stop(150);

    await robot.turnDegrees(((turnForce > 0) ? 80 : -80), 30, 10, true);

    readColors();
    while(!centerLeftBlack && !centerRightBlack){
        readColors();
        robot.turn(turnForce);
        await timer.delay();
    }
    await robot.stop();

    await returnRoutine();
    return true;
}

async Task<bool> checkTurn(){
    if(timer.current - lastTurnTime < 250)
        return false;

    if(gray)
        return true;

    if(await checkGreen())
        return true;

    int turnForce = 0;
    if(leftBlack){
        arrowLeds("Vermelho", 1);
        turnForce = -10;
    }
    else if (rightBlack){
        arrowLeds("Vermelho", 2);
        turnForce = 10;
    }
    else
        return false;

    if(leftBlack && rightBlack){
        arrowLeds("Vermelho", 1);
        arrowLeds("Vermelho", 2);
        await robot.moveStraightTime(15, 600, 1);
        await robot.stop(150);
        await returnRoutine();
    }

    await robot.stop(150);

    if(await checkGreen()){
        return true;
    }

    await robot.moveStraightTime(20, 400 , 1);
    await robot.stop(150);

    readColors();
    while(!centerLeftBlack && !centerRightBlack){
        readColors();
        robot.turn(turnForce);
        await timer.delay();
    }
    await robot.stop();

    await returnRoutine();
    return true;
}

async Task runLineFollower()
{
    if(centerLeftBlack){
        long timeout = timer.current + 350;

        while(timer.current < timeout)
        {
            readColors();
            if(await checkTurn())
                return;
            if(centerLeftLight > blackTreshold+diffForExit || centerRightBlack)
                break;
            robot.turn(-turnPower, 100);
            await timer.delay();
        }

    }

    else if(centerRightBlack){
        long timeout = timer.current + 350;

        while(timer.current < timeout)
        {
            readColors();
            if(await checkTurn())
                return;
            if(centerRightLight > blackTreshold+diffForExit || centerLeftBlack)
                break;
            robot.turn(turnPower, 100);
            await timer.delay();
        }

    }

    robot.moveStraight(targetPower);
}

async Task getLine(byte times = 3){
    for(int i = 0; i < times; i++){
        readColors(-10);
        long timeout = timer.current + 1500 + (300 * i);
        while(timer.current < timeout){
            readColors(-10);
            robot.turn(10);
            await timer.delay();

            if(leftBlack || centerLeftBlack || centerRightBlack || rightBlack){
                return;
            }
        }
        await robot.stop();

        timeout = timer.current + 3000 + (300 * i);
        while(timer.current < timeout){
            readColors(-10);
            robot.turn(-10);
            await timer.delay();

            if(leftBlack || centerLeftBlack || centerRightBlack || rightBlack){
                return;
            }
        }
        await robot.stop();
        await robot.moveStraightTime(10, 100);
    }

}


async Task<bool> checkObstacle(){
    if(interval(frontUltra[0].read, 0, 2.3f) || interval(frontUltra[1].read, 0, 2.3f)){
        turnOnAllLeds("Azul");
        await robot.stop(200);
        await robot.alignAngle();
        await robot.turnDegrees(70, 10);
        await robot.stop(200);
        await robot.moveStraightTime(10, 1800, 1);
        await robot.stop(200);
        await robot.turnDegrees(-70, 10);
        await robot.stop(200);
        await robot.alignAngle();
        await robot.moveStraightTime(10, 2000, 1);
        await robot.stop(200);
        await robot.turnDegrees(-75, 10);
        await robot.stop(200);
        turnOnAllLeds("Vermelho");
        readColors();
        while(!leftBlack && !centerLeftBlack && !centerRightBlack && !rightBlack){
            readColors();
            robot.moveStraight(10);
            await timer.delay();
        }
        turnOnAllLeds("Azul");
        await robot.moveStraightTime(10, 500, 1);
        await robot.stop(200);
        await robot.turnDegrees(50, 10, 10, true);
        turnOnAllLeds("Vermelho");
        while(!centerLeftBlack && !centerRightBlack){
            readColors();
            robot.turn(10);
            await timer.delay();
        }
        await robot.stop();
        turnOffAllLeds();
        await alignLine();
        return true;
    }
    return false;
}

async Task runFloor(){
    readColors();
    await checkObstacle();
    await runLineFollower();
    await checkTurn();
    await checkGreen();
    if(red){
        await robot.stop();
        turnOnAllLeds("Vermelho");
        IO.PrintLine("<b><size=12><align=center>That's all folks!</align></size></b>\n");
        await robot.die();
    }
}

const byte grayRed = 77;
const byte grayGreen = 85;
const byte grayBlue = 96;

double lastCompass = 0;
byte exitReason = 0;
/*
0 -> None
1 -> Triângulo (colisão)
2 -> Parede (ultra)
3 -> Timeout
*/

bool checkColision(){
    if(!proximity(robot.compass, lastCompass, 2) && Math.Abs(robot.compass - lastCompass) < 355){
        return true;
    }

    lastCompass = robot.compass;
    return false;
}

async Task findExit(){
    await robot.stop(150);
    await robot.alignAngle();

    await robot.moveStraightTime(30, 2500, 1);
    await robot.stop(300);
    await robot.alignAngle();
    await robot.stop(300);

    exitReason = 3;
    while(true){
        long timeout = timer.current + 9500;
        lastCompass = robot.compass;
        while(timer.current < timeout){
            robot.moveStraight(15);
            await timer.delay();

            if(interval(frontUltra[0].read, 0, 3.2f) || interval(frontUltra[1].read, 0, 3.2f)){
                exitReason = 2;
                break;
            }

            if(checkColision() && interval(frontUltra[1].read, 0, 12)){
                exitReason = 1;
                break;
            }

            if(lineSensors[0].isGreen && lineSensors[1].isGreen && lineSensors[2].isGreen && lineSensors[3].isGreen){
                await robot.stop(100);
                await robot.alignAngle();
                await robot.moveStraightTime(30, 150, 1);
                await returnRoutine();
                return;
            }

            if(leftUltra.read > 8 || leftUltra.read < 0){
                turnOnAllLeds("Azul");
                if(exitReason == 3)
                    await robot.moveStraightTime(10, 1750, 1);
                else
                    await robot.moveStraightTime(10, 1500, 1);

                await robot.stop(150);
                await robot.alignAngle();
                await robot.turnDegrees(-85, 20);
                await robot.alignAngle();
                turnOnAllLeds("Verde");
                while(!lineSensors[0].isGreen && !lineSensors[1].isGreen && !lineSensors[2].isGreen && !lineSensors[3].isGreen){
                    robot.moveStraight(15);
                    await timer.delay();
                }
                await timer.delay(300);
                await robot.stop(150);
                await returnRoutine();
                return;
            }
        }
        await robot.stop(150);
        await robot.moveStraightTime(-10, 500, 1);
        await robot.alignAngle();

        switch(exitReason){
            case 1:  // Triângulo
                await robot.turnDegrees(45, 30);
                await robot.moveStraightTime(30, 1250, 1);
                await robot.stop(150);
                await robot.turnDegrees(43, 30);
                await robot.alignAngle();
                await robot.stop(150);
                await robot.alignAngle();
                break;

            case 2:  // Parede
                await robot.moveStraightTime(10, 1400, 1);
                await robot.stop(150);
                await robot.moveStraightTime(-10, 1250, 1);
                await robot.alignAngle();
                await robot.turnDegrees(85, 30);
                await robot.alignAngle();
                await robot.stop(150);
                await robot.alignAngle();
                break;

            default:
                await robot.moveStraightTime(30, 100, 1);
                await returnRoutine();
                return;
        }
    }
}

bool afterRescue = false;

async Task setup()
{
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    timer.init();
    setGray(grayRed, grayGreen, grayBlue);
    await timer.delay(300);
    robot.locked = false;
    readColors();
    await alignLine();
    await robot.moveStraightTime(10, 300);
    readColors();
    await alignLine();
    IO.PrintLine("<color=#2aaae1><b><size=27><align=center>https://github.com/Eduardo-Barreto/VMRT-sBotics2022</align></size></b></color>\n");
}

async Task debugLoop()
{
    await getLine();
    await robot.die();
}

async Task loop()
{
    await runFloor();
    if(gray){
        await robot.stop();
        turnOnAllLeds(grayRed, grayGreen, grayBlue);
        await findExit();
        await robot.moveStraightTime(10, 200, 1);
        await getLine();
        gray = false;
        afterRescue = true;
    }
}

async Task Main()
{
    await setup();
    for (; ; )
    {
        #if(false)
            await debugLoop();
        #else
            await loop();
        #endif
        await timer.delay();
    }
}
