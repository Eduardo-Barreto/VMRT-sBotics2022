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