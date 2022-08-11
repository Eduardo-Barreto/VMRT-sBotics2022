import("config/createObjects.cs");
import("routines/lineFollower.cs");

async Task setup()
{
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    timer.init();
    await timer.delay(300);
    robot.locked = false;
    await robot.moveStraightTime(100, 300);
}

async Task loop()
{
    await runLineFollower();
}

async Task Main()
{
    await setup();
    for (; ; )
    {
        await loop();
        await timer.delay();
    }
}
