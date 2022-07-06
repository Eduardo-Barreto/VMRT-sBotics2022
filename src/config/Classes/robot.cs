
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