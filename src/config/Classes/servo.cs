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