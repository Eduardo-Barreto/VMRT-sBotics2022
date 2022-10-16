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

import("routines/crosspath.cs");

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

import("routines/obstacle.cs");

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