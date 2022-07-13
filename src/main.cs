import("config/createObjects.cs");
import("routines/lineFollower.cs");

void setup()
{
    robot.locked = false;
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
}


void loop()
{
    runLineFollower();
}

async Task Main()
{
    setup();
    for (; ; )
    {
        loop();
        await timer.delay();
    }
}

