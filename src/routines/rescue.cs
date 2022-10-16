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
