async Task<bool> checkObstacle(){
    if(interval(frontUltra[0].read, 0, 2.3f) || interval(frontUltra[1].read, 0, 2.3f)){
        await robot.stop(200);
        await robot.alignAngle();
        await robot.turnDegrees(90, 10);
        await robot.stop(200);
        await robot.alignAngle();
        await robot.moveStraightTime(10, 2000, 1);
        await robot.stop(200);
        await robot.turnDegrees(-90, 10);
        await robot.stop(200);
        await robot.alignAngle();
        await robot.moveStraightTime(10, 3000, 1);
        await robot.stop(200);
        await robot.turnDegrees(-90, 10);
        await robot.stop(200);
        await robot.alignAngle();
        readColors();
        while(!leftBlack && !centerLeftBlack && !centerRightBlack && !rightBlack){
            readColors();
            robot.moveStraight(10);
            await timer.delay();
        }
        foreach (led light in leds)
        {
            light.on();
        }
        await robot.moveStraightTime(10, 500, 1);
        await robot.stop(200);
        await robot.turnDegrees(90, 10);
        await robot.stop(200);
        await alignLine();
        foreach (led light in leds)
        {
            light.off();
        }
        return true;
    }
    return false;
}