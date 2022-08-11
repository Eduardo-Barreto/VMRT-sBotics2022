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
