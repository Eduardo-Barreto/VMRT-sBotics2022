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