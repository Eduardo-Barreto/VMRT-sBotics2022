import("config/math.cs")
import("config/createObjects.cs");
import("routines/leds.cs");
import("routines/lineFollower.cs");

async Task setup()
{
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    timer.init();
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
}

async Task loop()
{
    await runFloor();
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
