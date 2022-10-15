import("config/math.cs")
import("config/createObjects.cs");
import("routines/leds.cs");
import("routines/lineFollower.cs");
import("routines/rescue.cs");

bool afterRescue = false;

async Task setup()
{
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    timer.init();
    setGray(grayRed, grayGreen, grayBlue);
    await timer.delay(300);
    robot.locked = false;
    readColors();
    await alignLine();
    await robot.moveStraightTime(10, 300);
    readColors();
    await alignLine();
}

async Task debugLoop()
{
    await robot.alignAngle();
    await robot.alignUltra(2, 100, 1);
    IO.PrintLine("Ultra: " + frontUltra[0].read.ToString());
    await robot.die();
}

async Task loop()
{
    await runFloor();
    if(gray){
        await robot.stop();
        turnOnAllLeds(grayRed, grayGreen, grayBlue);
        await findExit();
        gray = false;
        afterRescue = true;
    }
}

async Task Main()
{
    await setup();
    for (; ; )
    {
        #if(false)
            await debugLoop();
        #else
            await loop();
        #endif
        await timer.delay();
    }
}
