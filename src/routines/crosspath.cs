long lastTurnTime = 0;

async Task returnRoutine(){
    await robot.stop();
    readColors();
    await alignLine();

    turnOffAllLeds();
    lastTurnTime = timer.current;
    return;
}

async Task<bool> checkDeadEnd(){
    if(leftGreen && rightGreen){
        arrowLeds("Verde", 1);
        arrowLeds("Verde", 2);
        await robot.moveStraightTime(15, 700, 1);
        await robot.stop(150);

        await robot.turnDegrees(170, 30, 10, true);

        readColors();
        while(!centerLeftBlack && !centerRightBlack){
            readColors();
            robot.turn(10);
            await timer.delay();
        }
        await robot.stop();

        await returnRoutine();
        return true;
    }
    return false;
}

async Task <bool> checkGreen(){
    if(timer.current - lastTurnTime < 750)
        return false;

    int turnForce = 0;
    if(leftGreen){
        turnForce = -10;
        arrowLeds("Verde", 1);
    }
    else if(rightGreen){
        turnForce = 10;
        arrowLeds("Verde", 2);
    }
    else
        return false;

    await robot.stop(150);

    if(await checkDeadEnd()){
        return true;
    }

    readColors();

    if(await checkDeadEnd()){
        return true;
    }

    await robot.moveStraightTime(15, 700, 1);
    await robot.stop(150);

    await robot.turnDegrees(((turnForce > 0) ? 80 : -80), 30, 10, true);

    readColors();
    while(!centerLeftBlack && !centerRightBlack){
        readColors();
        robot.turn(turnForce);
        await timer.delay();
    }
    await robot.stop();

    await returnRoutine();
    return true;
}

async Task<bool> checkTurn(){
    if(timer.current - lastTurnTime < 250)
        return false;

    if(await checkGreen()){
        return true;
    }

    int turnForce = 0;
    if(leftBlack){
        arrowLeds("Vermelho", 1);
        turnForce = -10;
    }
    else if (rightBlack){
        arrowLeds("Vermelho", 2);
        turnForce = 10;
    }
    else
        return false;

    if(leftBlack && rightBlack){
        arrowLeds("Vermelho", 1);
        arrowLeds("Vermelho", 2);
        await robot.moveStraightTime(15, 600, 1);
        await robot.stop(150);
        await returnRoutine();
    }

    await robot.stop(150);

    if(await checkGreen()){
        return true;
    }

    await robot.moveStraightTime(20, 400 , 1);
    await robot.stop(150);

    readColors();
    while(!centerLeftBlack && !centerRightBlack){
        readColors();
        robot.turn(turnForce);
        await timer.delay();
    }
    await robot.stop();

    await returnRoutine();
    return true;
}
