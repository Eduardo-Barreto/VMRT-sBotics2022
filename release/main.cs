

// Servomotor leftMotor = Bot.GetComponent<Servomotor>("leftMotor");

public class motor
{
    private Servomotor servo;

    public motor(string motorName)
    {
        this.servo = Bot.GetComponent<Servomotor>(motorName);
    }

    public bool locked
    {
        get => servo.Locked;
        set => servo.Locked = value;
    }

    public double angle
    {
        get => servo.Angle;
    }

    public double velocity
    {
        get => servo.Velocity;
        set => servo.Target = value;
    }

    public double force
    {
        get => servo.Force;
        set => servo.Force = value;
    }

    public void run(double _velocity, double _force = 500)
    {
        this.velocity = _velocity;
        this.force = _force;
    }

    public string test = "abacate";
};

motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");



public class robotBase
{
    private motor leftMotor, rightMotor;
    public robotBase(string leftMotorName, string rightMotorName)
    {
        this.leftMotor = new motor(leftMotorName);
        this.rightMotor = new motor(rightMotorName);
    }
    public void move(double leftVelocity, double rightVelocity, double leftForce = 500, double rightForce = 500)
    {
        leftMotor.run(leftVelocity, leftForce);
        rightMotor.run(rightVelocity, rightForce);
    }

    public void turn(double velocity, double force = 500)
    {
        rightMotor.run(-velocity, force);
        leftMotor.run(velocity, force);
    }

    public void moveStraight(double velocity, double force = 500)
    {
        rightMotor.run(velocity, force);
        leftMotor.run(velocity, force);
    }

    public void stop(bool _lock = true)
    {
        leftMotor.run(0, 0);
        rightMotor.run(0, 0);

        leftMotor.locked = _lock;
        rightMotor.locked = _lock;
    }

    public bool locked
    {
        get => this.locked;
        set
        {
            leftMotor.locked = value;
            rightMotor.locked = value;
        }
    }

};
robotBase robot = new robotBase("leftMotor", "rightMotor");


public class lightSensor
{
    private ColorSensor sensor;

    public lightSensor(string sensorName)
    {
        this.sensor = Bot.GetComponent<ColorSensor>(sensorName);
    }

    public double raw
    {
        get => sensor.Analog.Brightness;
    }

    public double light
    {
        get => 100 - raw;
    }

    public double red
    {
        get => sensor.Analog.Red;
    }

    public double green
    {
        get => sensor.Analog.Green;
    }

    public double blue
    {
        get => sensor.Analog.Blue;
    }

    public string color
    {
        get => sensor.Analog.ToString();
    }

    public bool isColorBlack
    {
        get => !sensor.Digital;
    }
};
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3"),
    new lightSensor("S4")
};

double error, lastError, P, D, PD;
int targetPower = 450;
long counter = 0;

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

byte sens = 15;
async void runLineFollower()
{
    if ((lineSensors[1].light > lineSensors[3].light + sens) || (lineSensors[0].light > lineSensors[4].light + sens))
    {
        robot.move(-500, 500);
    }
    else if ((lineSensors[3].light > lineSensors[1].light + sens) || (lineSensors[4].light > lineSensors[0].light + sens))

    {
        robot.move(500, -500);
    }
    else
    {
        robot.moveStraight(500);
    }
}

double startTime;
void setup()
{
    robot.locked = false;
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    startTime = Time.Timestamp;
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
        await Task.Delay(50);
    }
}
