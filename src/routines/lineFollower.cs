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

void readColors(){
    leftLight           = (byte)(lineSensors[0].light);
    centerLeftLight     = (byte)(lineSensors[1].light);
    centerRightLight    = (byte)(lineSensors[2].light);
    rightLight          = (byte)(lineSensors[3].light);

    leftBlack           = (leftLight < blackTresholdTurn);
    centerLeftBlack     = (centerLeftLight < blackTreshold);
    centerRightBlack    = (centerRightLight < blackTreshold);
    rightBlack          = (rightLight < blackTresholdTurn);

    leftGreen           = (lineSensors[0].isGreen || lineSensors[1].isGreen);
    rightGreen          = (lineSensors[2].isGreen || lineSensors[3].isGreen);
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

import("routines/obstacle.cs");

async Task runFloor(){
    readColors();
    await checkObstacle();
    await runLineFollower();
    await checkTurn();
    await checkGreen();
}