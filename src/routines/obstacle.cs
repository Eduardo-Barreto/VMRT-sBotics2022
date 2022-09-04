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
        await robot.moveStraightTime(10, 2000, 1);
        await robot.stop(200);
        await robot.turnDegrees(-75, 10);
        await robot.stop(200);
        turnOnAllLeds("Vermelho");
        readColors();
        while(!leftBlack && !centerLeftBlack && !centerRightBlack && !rightBlack){
            readColors();
            robot.moveStraight(10);
            await timer.delay();
        }
        turnOnAllLeds("Azul");
        await robot.moveStraightTime(10, 500, 1);
        await robot.stop(200);
        await robot.turnDegrees(50, 10, 10, true);
        turnOnAllLeds("Vermelho");
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