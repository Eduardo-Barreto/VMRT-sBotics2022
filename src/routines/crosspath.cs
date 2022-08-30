long lastTurnTime = 0;

async Task returnRoutine(){
    await robot.stop();
    readColors();
    await alignLine();

    leds[0].off();
    leds[1].off();
    leds[2].off();
    leds[3].off();
    lastTurnTime = timer.current;
    return;
}

async Task<bool> checkDeadEnd(){
    if(leftGreen && rightGreen){
        leds[0].on("Verde");
        leds[3].on("Verde");
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
        leds[0].on("Verde");
    }
    else if(rightGreen){
        turnForce = 10;
        leds[3].on("Verde");
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
        leds[0].on();
        turnForce = -10;
    }
    else if (rightBlack){
        leds[3].on();
        turnForce = 10;
    }
    else
        return false;

    await robot.stop(150);

    if(await checkGreen()){
        return true;
    }

    if(leftBlack && rightBlack){
        leds[0].on();
        leds[3].on();
        await robot.moveStraightTime(15, 600, 1);
        await robot.stop(150);
        await returnRoutine();
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