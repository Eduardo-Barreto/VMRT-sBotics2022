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

        // Check 90 direita
        long timeout = timer.current + 2000;
        while(timer.current < timeout){
            robot.moveStraight(10);
            await timer.delay();

            if(checkAnyBlack()){
                await robot.stop();
                turnOnAllLeds("Vermelho");
                await robot.moveStraightTime(15, 400, 1);
                await robot.stop(150);
                await robot.turnDegrees(45, 10);
                await robot.moveStraightTime(15, 300, 1);
                while(!centerLeftBlack && !centerRightBlack){
                    readColors();
                    robot.turn(10);
                    await timer.delay();
                }
                await returnRoutine();
                return true;
            }
        }

        await robot.stop(200);
        await robot.turnDegrees(-75, 10);
        await robot.stop(200);
        readColors();
        timeout = timer.current + 2000;
        while(!leftBlack && !centerLeftBlack && !centerRightBlack && !rightBlack){
            readColors();
            robot.moveStraight(10);
            await timer.delay();
            if(timer.current > timeout){
                turnOnAllLeds("Vermelho");
                await robot.stop(200);
                await robot.turnDegrees(-30, 10);
                await robot.stop(200);
                await robot.moveStraightTime(15, 550, 1);
                await robot.stop(200);
                await robot.turnDegrees(-45, 10);
                readColors();
                while(!centerLeftBlack && !centerRightBlack){
                    readColors();
                    robot.moveStraight(10);
                    await timer.delay();
                }
                await robot.moveStraightTime(15, 300, 1);
                readColors();
                while(!centerLeftBlack && !centerRightBlack){
                    readColors();
                    robot.turn(10);
                    await timer.delay();
                }
                await returnRoutine();
                await robot.stop();
                await returnRoutine();
                return false;
            }
        }
        turnOnAllLeds("Vermelho");
        await robot.moveStraightTime(10, 500, 1);
        await robot.stop(200);
        await robot.turnDegrees(50, 10, 10, true);
        readColors();
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